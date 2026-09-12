using Syntac.DI.Core.Installers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Syntac.DI.Core.Scopes
{
    /// <summary>
    /// The application root scope. Instantiated by <see cref="DIBootStrapper"/> from
    /// <c>Resources/DI/ProjectLifetimeScope</c>.
    /// </summary>
    /// <remarks>
    /// The <c>s_Initialized</c> guard prevents double registration if a second instance is ever created.
    /// Editor requirement: enable "Enter Play Mode Options → Reload Domain", or this static flag (and
    /// <c>DIBootStrapper.s_Initialized</c>) survives between Play sessions and the second run registers
    /// nothing. If domain reload must stay disabled, add
    /// <c>[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]</c> reset
    /// methods that set both flags back to false.
    /// </remarks>
    public sealed class ProjectLifetimeScope : LifetimeScope
    {
        [SerializeField] private MonoInstaller[] m_Installers;

        private static bool s_Initialized;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            if (s_Initialized)
                return;

            foreach (MonoInstaller installer in m_Installers)
                installer.Install(builder);

            s_Initialized = true;
        }
    }
}
