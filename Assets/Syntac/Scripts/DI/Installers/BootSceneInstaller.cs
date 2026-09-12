using System.Collections.Generic;
using Syntac.DI.Core.Installers;
using Syntac.MessagePipe.Installers;
using VContainer;
using VContainer.Unity;

namespace Syntac.DI.Installers
{
    /// <summary>
    /// Boot scene wiring. The FetchInstallers() iterator is the extension seam: adding a system to this
    /// scene is one <c>yield return</c>.
    /// </summary>
    public class BootSceneInstaller : MonoInstaller
    {
        public override void Install(IContainerBuilder builder)
        {
            foreach (IInstaller i in FetchInstallers())
            {
                i.Install(builder);
            }
        }

        private IEnumerable<IInstaller> FetchInstallers()
        {
            yield return new BootPipeInstaller();
        }
    }
}
