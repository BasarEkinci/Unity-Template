using JetBrains.Annotations;
using Syntac.SettingsSystem.Core;
using UnityEngine;

namespace Syntac.SettingsSystem.Graphics
{
    [UsedImplicitly]
    public class VSyncSetting : SettingsBase<bool>
    {
        public override string Key => "vsync";

        protected override bool GetDefault() => true;

        public override void Apply() => QualitySettings.vSyncCount = Value ? 1 : 0;

        public override void Load(string rawValue)
        {
            bool isValueOne = rawValue == "1";
            bool isValueTrue = rawValue.ToLower() == "true";
            Set(isValueOne || isValueTrue);
        }

        public override string Serialize() => Value ? "1" : "0";
    }
}
