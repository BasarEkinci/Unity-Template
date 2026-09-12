using System;

namespace Syntac.SettingsSystem.Core
{
    /// <summary>
    /// The non-generic face of a setting: what the service stores and iterates as a heterogeneous
    /// collection.
    /// </summary>
    public interface ISettings
    {
        event Action Changed;
        string Key { get; }
        object Value { get; }
        void Set(object value);
        void Load(string rawValue);
        void Apply();
        string Serialize();
        void Reset();
    }

    /// <summary>
    /// The generic face of a setting: what consumers resolve for type-safe access.
    /// <c>new</c> hides the non-generic <c>Value</c>/<c>Changed</c> members.
    /// </summary>
    public interface ISettings<T> : ISettings
    {
        new event Action<T> Changed;
        new T Value { get; }
        void Set(T value);
    }
}
