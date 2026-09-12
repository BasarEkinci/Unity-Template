using System;
using Syntac.DI.Core.Scopes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Syntac.DI.Core
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Bootstrapper for Dependency Injection system.
    /// </summary>
    /// <remarks>
    /// The root scope exists in every Play-mode session regardless of which scene Play was pressed in —
    /// this is what makes "press Play in any scene" work. <c>hideFlags = NotEditable</c> means the runtime
    /// instance cannot be edited in the Inspector; edit the prefab instead. <c>Resources.Load</c> is
    /// synchronous and happens before the first scene, so keep the prefab lightweight.
    /// </remarks>
    internal static class DIBootStrapper
    {
        private static bool s_Initialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (s_Initialized)
                return;

            ProjectLifetimeScope scopeResource = Resources.Load<ProjectLifetimeScope>("DI/ProjectLifetimeScope");

            if (scopeResource == null)
                throw new NullReferenceException("Ensure that the ProjectLifetimeScope resource exists at 'Resources/DI/ProjectLifetimeScope'.");

            ProjectLifetimeScope scopeInstance = Object.Instantiate(scopeResource);
            GameObject scopeGo = scopeInstance.gameObject;
            scopeGo.name = $"[{nameof(ProjectLifetimeScope)}]";
            scopeGo.hideFlags = HideFlags.NotEditable;

            Object.DontDestroyOnLoad(scopeGo);
            s_Initialized = true;
        }
    }
}
