using Syntac.DI.Core.Installers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Syntac.DI.Core.Scopes
{
    /// <summary>
    /// A child scope, one per scene. Beyond running its installers it walks the scene and injects every
    /// MonoBehaviour — including inactive ones — so scene-authored components can use [Inject] fields and
    /// methods without being registered in the container.
    /// </summary>
    public sealed class SceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private MonoInstaller[] m_MonoInstallers;

        protected override void Awake()
        {
            base.Awake();
            AutoInjectSceneObjects();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            foreach (IInstaller installer in m_MonoInstallers)
                installer.Install(builder);
        }

        /// <remarks>
        /// Performance warning: this is O(all components in scene) and runs reflection-based injection on
        /// each. It is fine for small/medium scenes and for a jam-scale project; for a large scene,
        /// replace it with an explicit registration list.
        ///
        /// Auto-injection caveat: objects instantiated *after* Awake are not injected. Use
        /// <c>Container.Instantiate(prefab)</c> or <c>IObjectResolver.Inject(instance)</c> for runtime spawns.
        /// </remarks>
        private void AutoInjectSceneObjects()
        {
            MonoBehaviour[] allMonoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            for (int i = 0; i < allMonoBehaviours.Length; i++)
            {
                MonoBehaviour mb = allMonoBehaviours[i];
                Container.Inject(mb);
            }
        }
    }
}
