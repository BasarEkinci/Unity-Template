using Syntac.SettingsSystem.Audio;
using Syntac.SettingsSystem.Core;
using Syntac.SettingsSystem.Graphics;
using VContainer;
using VContainer.Unity;

namespace Syntac.SettingsSystem.DI
{
    /// <summary>
    /// VContainer installer for the Settings System.
    /// </summary>
    /// <remarks>
    /// <c>AsSelf().As&lt;ISettings&gt;()</c> registers each setting under both its concrete type (so UI can
    /// inject <c>MasterVolumeSetting</c> directly) and the interface (so the service's
    /// <c>IEnumerable&lt;ISettings&gt;</c> collects it).
    ///
    /// <c>RegisterInstance(m_Config.MainMixer)</c> is what makes the audio settings' AudioMixer
    /// constructor parameter resolvable. If MainMixer is null in the config, container construction throws.
    /// </remarks>
    public class SettingsInstaller : IInstaller
    {
        private readonly SettingsSystemConfig m_Config;

        public SettingsInstaller(SettingsSystemConfig config)
        {
            m_Config = config;
        }

        public void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(m_Config.MainMixer);

            // Core Audio Settings
            builder.Register<MasterVolumeSetting>(Lifetime.Singleton).AsSelf().As<ISettings>();
            builder.Register<BGMVolumeSetting>(Lifetime.Singleton).AsSelf().As<ISettings>();
            builder.Register<SFXVolumeSetting>(Lifetime.Singleton).AsSelf().As<ISettings>();
            builder.Register<VoiceVolumeSetting>(Lifetime.Singleton).AsSelf().As<ISettings>();
            builder.Register<AmbienceVolumeSetting>(Lifetime.Singleton).AsSelf().As<ISettings>();

            // Core Graphics Settings
            builder.Register<ResolutionSetting>(Lifetime.Singleton).AsSelf().As<ISettings>();
            builder.Register<FullscreenSetting>(Lifetime.Singleton).AsSelf().As<ISettings>();
            builder.Register<VSyncSetting>(Lifetime.Singleton).AsSelf().As<ISettings>();
            builder.Register<QualityLevelSetting>(Lifetime.Singleton).AsSelf().As<ISettings>();
            builder.Register<TargetFramerateSetting>(Lifetime.Singleton).AsSelf().As<ISettings>();

            // Custom Providers
            foreach (ISettingsProvider provider in m_Config.CustomProviders)
                provider.Register(builder);

            // Service
            builder.RegisterEntryPoint<SettingsService>().As<ISettingsService>();
        }
    }
}
