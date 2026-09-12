using System.Globalization;
using Syntac.SettingsSystem.Core;
using UnityEngine;
using UnityEngine.Audio;
using VContainer;

namespace Syntac.SettingsSystem.Audio
{
    /// <summary>
    /// Linear 0..1 volume mapped to decibels via log10(v) * 20, with a hard -80 dB floor at zero
    /// (Unity's mixer minimum).
    /// </summary>
    /// <remarks>
    /// The AudioMixer asset must expose a parameter named exactly "SFXVolume". A missing parameter makes
    /// SetFloat silently return false â€” no error. CultureInfo.InvariantCulture is mandatory on both
    /// parse and serialize, otherwise a machine with a comma decimal separator writes "0,5" and a machine
    /// with a dot fails to read it.
    /// </remarks>
    public sealed class SFXVolumeSetting : SettingsBase<float>
    {
        private const string k_ExposedParam = "SFXVolume";
        private readonly AudioMixer m_Mixer;

        [Inject]
        public SFXVolumeSetting(AudioMixer mixer)
        {
            m_Mixer = mixer;
        }

        public override string Key => "sfx_volume";

        protected override float GetDefault() => 1f;

        public override void Apply()
        {
            if (m_Mixer == null)
                return;

            float db = Value > 0 ? Mathf.Log10(Value) * 20f : -80f;
            m_Mixer.SetFloat(k_ExposedParam, db);
        }

        public override void Load(string rawValue)
        {
            if (float.TryParse(rawValue, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
                Set(Mathf.Clamp01(value));
        }

        public override string Serialize() => Value.ToString(CultureInfo.InvariantCulture);
    }
}