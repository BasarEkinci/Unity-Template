using JetBrains.Annotations;
using Syntac.SettingsSystem.Core;
using UnityEngine;

namespace Syntac.SettingsSystem.Graphics
{
    [UsedImplicitly]
    public class ResolutionSetting : SettingsBase<Vector2Int>
    {
        public override string Key => "resolution";

        protected override Vector2Int GetDefault()
        {
            Resolution current = Screen.currentResolution;
            return new(current.width, current.height);
        }

        public override void Apply() => Screen.SetResolution(Value.x, Value.y, Screen.fullScreenMode);

        public override void Load(string rawValue)
        {
            string[] parts = rawValue.Split('x');

            // Bounds check before indexing parts[1]: a malformed value like "1920" would otherwise throw.
            if (parts.Length != 2)
                return;

            if (int.TryParse(parts[0], out int width) && int.TryParse(parts[1], out int height))
                Set(new(width, height));
        }

        public override string Serialize() => $"{Value.x}x{Value.y}";
    }
}
