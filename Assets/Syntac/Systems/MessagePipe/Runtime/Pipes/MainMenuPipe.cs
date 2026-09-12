using JetBrains.Annotations;
using Syntac.Signals;

namespace Syntac.MessagePipe.Pipes
{
    /// <summary>
    /// Main-menu bus: menu navigation, UI events.
    /// Must be registered as a singleton in a scope so it is disposed with that scope.
    /// </summary>
    [UsedImplicitly]
    public class MainMenuPipe : GenericEventBus<ISignal>
    {
    }
}
