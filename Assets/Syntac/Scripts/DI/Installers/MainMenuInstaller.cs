using System.Collections.Generic;
using Syntac.DI.Core.Installers;
using Syntac.MessagePipe.Installers;
using VContainer;
using VContainer.Unity;

namespace Syntac.DI.Installers
{
    /// <summary>
    /// Main menu scene wiring. The FetchInstallers() iterator is the extension seam: adding a system to
    /// this scene is one <c>yield return</c>.
    /// </summary>
    public class MainMenuInstaller : MonoInstaller
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
            yield return new MainMenuPipeInstaller();
        }
    }
}
