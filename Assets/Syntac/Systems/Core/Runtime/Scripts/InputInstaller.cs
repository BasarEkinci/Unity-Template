using VContainer;
using VContainer.Unity;

namespace Syntac.Core
{
    public class InputInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register
            (
                _ =>
                {
                    InputSchema schema = new();
                    schema.Enable();
                    return schema;
                },
                Lifetime.Singleton
            );
        }
    }
}
