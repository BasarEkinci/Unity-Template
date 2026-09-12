using Syntac.MessagePipe.Pipes;
using VContainer;
using VContainer.Unity;

namespace Syntac.MessagePipe.Installers
{
    public class MainMenuPipeInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<MainMenuPipe>(Lifetime.Singleton);
        }
    }
}
