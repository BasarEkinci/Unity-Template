using UnityEngine;
using VContainer;

namespace Syntac.SettingsSystem.DI
{
    /// <summary>
    /// Extension point: lets a game add its own settings from a ScriptableObject dropped into the config
    /// asset, with no edit to the framework installer.
    /// </summary>
    public interface ISettingsProvider
    {
        void Register(IContainerBuilder builder);
    }

    /// <summary>
    /// ScriptableObject base for <see cref="ISettingsProvider"/>. Create an asset from a subclass and drag
    /// it into <c>SettingsSystemConfig.CustomProviders</c>.
    /// </summary>
    public abstract class SettingsProviderBase : ScriptableObject, ISettingsProvider
    {
        public abstract void Register(IContainerBuilder builder);
    }
}
