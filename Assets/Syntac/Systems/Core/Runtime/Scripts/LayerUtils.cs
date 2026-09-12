using UnityEngine;

namespace Syntac.Core
{
    public static class LayerUtils
    {
        private static readonly LayerMask s_Default = LayerMask.NameToLayer("Default");                        // 0
        private static readonly LayerMask s_TransparentFX = LayerMask.NameToLayer("TransparentFX");            // 1
        private static readonly LayerMask s_IgnoreRaycastLayerMask = LayerMask.NameToLayer("Ignore Raycast");  // 2
        private static readonly LayerMask s_Player = LayerMask.NameToLayer("Player");                          // 3
        private static readonly LayerMask s_Water = LayerMask.NameToLayer("Water");                            // 4
        private static readonly LayerMask s_UI = LayerMask.NameToLayer("UI");                                  // 5
        private static readonly LayerMask s_IgnorePlayer = LayerMask.NameToLayer("Ignore Player");             // 7
        private static readonly LayerMask s_Nothing = 0;
        private static readonly LayerMask s_Everything = ~0;

        public static LayerMask Default => s_Default;
        public static LayerMask TransparentFX => s_TransparentFX;
        public static LayerMask IgnoreRaycast => s_IgnoreRaycastLayerMask;
        public static LayerMask Player => s_Player;
        public static LayerMask Water => s_Water;
        public static LayerMask UI => s_UI;
        public static LayerMask IgnorePlayer => s_IgnorePlayer;
        public static LayerMask Nothing => s_Nothing;
        public static LayerMask Everything => s_Everything;
    }
}
