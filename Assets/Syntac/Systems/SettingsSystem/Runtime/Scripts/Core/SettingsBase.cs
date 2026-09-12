using System;
using System.Collections.Generic;

namespace Syntac.SettingsSystem.Core
{
    /// <summary>
    /// Base for a typed setting that knows its own key, default, how to apply itself to the engine,
    /// and how to (de)serialize itself.
    /// </summary>
    /// <remarks>
    /// The default is lazy: <see cref="GetDefault"/> is not called in a constructor, so a setting can
    /// read engine state (current resolution, current quality level) at first access rather than at
    /// container-build time.
    ///
    /// Behavioural contract: <see cref="Set(T)"/> is a no-op when the value is unchanged, so
    /// <see cref="Apply"/> and <c>Changed</c> do not fire redundantly. Subclasses implement
    /// <see cref="Load"/> by parsing and calling <see cref="Set(T)"/> — never by writing the backing
    /// field directly — which keeps apply/notify consistent between the disk-load and UI-change paths.
    /// </remarks>
    public abstract class SettingsBase<T> : ISettings<T>
    {
        private Action<T> m_OnChanged;
        private Action m_OnChangedNonGeneric;
        private T m_Value;
        private bool m_IsInitialized;

        public abstract string Key { get; }

        public T Value
        {
            get
            {
                if (m_IsInitialized)
                    return m_Value;

                m_Value = GetDefault();
                m_IsInitialized = true;
                return m_Value;
            }
        }

        object ISettings.Value => Value;

        public event Action<T> Changed
        {
            add => m_OnChanged += value;
            remove => m_OnChanged -= value;
        }

        event Action ISettings.Changed
        {
            add => m_OnChangedNonGeneric += value;
            remove => m_OnChangedNonGeneric -= value;
        }

        public virtual void Set(T value)
        {
            if (EqualityComparer<T>.Default.Equals(Value, value))
                return;

            m_Value = value;
            m_IsInitialized = true;
            Apply();
            m_OnChanged?.Invoke(Value);
            m_OnChangedNonGeneric?.Invoke();
        }

        void ISettings.Set(object value) => Set((T)value);

        public abstract void Load(string rawValue);

        public void Reset()
        {
            T def = GetDefault();
            Set(def);
        }

        public virtual string Serialize() => Value?.ToString() ?? string.Empty;

        public abstract void Apply();
        protected abstract T GetDefault();
    }
}
