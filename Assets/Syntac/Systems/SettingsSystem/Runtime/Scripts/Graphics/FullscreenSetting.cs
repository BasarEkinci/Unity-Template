using System;
using JetBrains.Annotations;
using Syntac.SettingsSystem.Core;
using UnityEngine;

namespace Syntac.SettingsSystem.Graphics
{
    [UsedImplicitly]
    public class FullscreenSetting : SettingsBase<FullScreenMode>
    {
        public override string Key => "fullscreen_mode";

        protected override FullScreenMode GetDefault() => FullScreenMode.FullScreenWindow;

        public override void Apply() => Screen.fullScreenMode = Value;

        public override void Load(string rawValue)
        {
            if (int.TryParse(rawValue, out int intValue) && Enum.IsDefined(typeof(FullScreenMode), intValue))
            {
                Set((FullScreenMode)intValue);
            }
        }

        public override string Serialize() => ((int)Value).ToString();
    }
}
