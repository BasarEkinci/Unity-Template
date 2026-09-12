using Syntac.Core;

namespace Syntac.SettingsSystem
{
    public static class EnvironmentVariables
    {
        public const string SystemName = "Settings System";
        public const string Version = "1.0.0";
        public const string Author = "Syntac";

        public const string MenuRoot = GlobalEnvironmentVariables.AppName + "/" + SystemName + "/";
        public const string ConfigMenuPath = MenuRoot + "Config";
        public const string EditorWindowMenuPath = "Window/" + MenuRoot + "Settings";

        public const string SettingsFileName = "settings.cfg";

        public const string LogPrefix = "[Settings]";
    }
}
