using Syntac.MessagePipe.Pipes;
using VContainer;
using VContainer.Unity;

namespace Syntac.MessagePipe.Installers
{
    public class BootPipeInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<BootPipe>(Lifetime.Singleton);
        }
    }
}
