using JetBrains.Annotations;
using Syntac.Signals;

namespace Syntac.MessagePipe.Pipes
{
    /// <summary>
    /// Boot-scene bus: loading progress, boot step completion.
    /// Must be registered as a singleton in a scope so it is disposed with that scope.
    /// </summary>
    [UsedImplicitly]
    public class BootPipe : GenericEventBus<ISignal>
    {
    }
}
