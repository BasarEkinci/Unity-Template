using JetBrains.Annotations;
using Syntac.SettingsSystem.Core;
using UnityEngine;

namespace Syntac.SettingsSystem.Graphics
{
    // Inherits SettingsBase.Serialize() (Value.ToString()), which is correct for int.
    // Apply-order note: with vSync on, Application.targetFrameRate is ignored. See SettingsService.Load.
    [UsedImplicitly]
    public class TargetFramerateSetting : SettingsBase<int>
    {
        public override string Key => "target_framerate";

        protected override int GetDefault() => 60;

        public override void Apply() => Application.targetFrameRate = Value;

        public override void Load(string rawValue)
        {
            if (int.TryParse(rawValue, out int value))
                Set(Mathf.Clamp(value, -1, 300));
        }
    }
}
