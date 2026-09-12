using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Syntac.DI.Core.Installers
{
    /// <summary>
    /// Installers are MonoBehaviours so they can hold [SerializeField] references to assets
    /// (configs, prefabs, mixers) that a plain C# installer cannot.
    /// </summary>
    public abstract class MonoInstaller : MonoBehaviour, IInstaller
    {
        public abstract void Install(IContainerBuilder builder);
    }
}
