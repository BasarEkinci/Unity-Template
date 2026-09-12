using JetBrains.Annotations;
using Syntac.SettingsSystem.Core;
using UnityEngine;

namespace Syntac.SettingsSystem.Graphics
{
    // Inherits SettingsBase.Serialize() (Value.ToString()), which is correct for int.
    [UsedImplicitly]
    public class QualityLevelSetting : SettingsBase<int>
    {
        public override string Key => "quality_level";

        protected override int GetDefault() => QualitySettings.GetQualityLevel();

        public override void Apply()
        {
            QualitySettings.SetQualityLevel(Value, true);
        }

        public override void Load(string rawValue)
        {
            if (!int.TryParse(rawValue, out int value))
                return;

            int maxLevel = QualitySettings.names.Length - 1;
            Set(Mathf.Clamp(value, 0, maxLevel));
        }
    }
}
