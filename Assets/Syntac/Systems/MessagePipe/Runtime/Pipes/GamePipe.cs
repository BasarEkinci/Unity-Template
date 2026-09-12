using JetBrains.Annotations;
using Syntac.Signals;

namespace Syntac.MessagePipe.Pipes
{
    /// <summary>
    /// Gameplay bus: damage, pickup, death.
    /// Must be registered as a singleton in a scope so it is disposed with that scope.
    /// </summary>
    [UsedImplicitly]
    public class GamePipe : GenericEventBus<ISignal>
    {
    }
}
