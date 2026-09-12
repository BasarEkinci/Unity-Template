using System.Collections.Generic;
using Syntac.Core;
using Syntac.DI.Core.Installers;
using Syntac.MessagePipe.Installers;
using Syntac.SettingsSystem.DI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Syntac.DI.Installers
{
    /// <summary>
    /// Root wiring. The only place where framework modules are composed.
    /// </summary>
    /// <remarks>
    /// The <c>yield return</c> sequence *is* the registration order. Registration order does not matter
    /// for resolution (VContainer builds the graph after all registrations), but entry-point
    /// <c>Start()</c> order follows registration order — keep <see cref="InputInstaller"/> first and pipes
    /// last.
    /// </remarks>
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private SettingsSystemConfig m_SettingsSystemConfig;

        public override void Install(IContainerBuilder builder)
        {
            foreach (IInstaller i in FetchInstallers())
                i.Install(builder);
        }

        private IEnumerable<IInstaller> FetchInstallers()
        {
            yield return new InputInstaller();
            yield return new SettingsInstaller(m_SettingsSystemConfig);
            yield return new ProjectPipeInstaller();
        }
    }
}
