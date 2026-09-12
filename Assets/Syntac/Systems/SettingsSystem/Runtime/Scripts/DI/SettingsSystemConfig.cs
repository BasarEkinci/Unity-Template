using System.Collections.Generic;
using Syntac.ConfigManagement;
using UnityEngine;
using UnityEngine.Audio;

namespace Syntac.SettingsSystem.DI
{
    /// <summary>
    /// Configuration asset for the Settings System.
    /// Create via: Assets -> Create -> Syntac/Settings System/Config
    /// </summary>
    [CreateAssetMenu(fileName = nameof(SettingsSystemConfig), menuName = EnvironmentVariables.ConfigMenuPath)]
    public class SettingsSystemConfig : ScriptableObject, IVisibleConfig
    {
        [SerializeField] private AudioMixer m_MainMixer;
        [SerializeField] private List<SettingsProviderBase> m_CustomProviders = new();

        public AudioMixer MainMixer => m_MainMixer;
        public IEnumerable<ISettingsProvider> CustomProviders => m_CustomProviders;

        string IVisibleConfig.ConfigName => EnvironmentVariables.SystemName;
        string IVisibleConfig.Category => "System";
    }
}
