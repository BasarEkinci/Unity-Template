using System.Collections.Generic;
using System.IO;
using System.Linq;
using Syntac.Core;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Syntac.SettingsSystem.Core
{
    /// <summary>
    /// Aggregates every registered <see cref="ISettings"/> and persists them to disk.
    /// </summary>
    /// <remarks>
    /// VContainer injects <c>IEnumerable&lt;ISettings&gt;</c> — every setting registered
    /// <c>.As&lt;ISettings&gt;()</c> arrives automatically, so adding a new setting requires only a
    /// registration line and no change here.
    ///
    /// Registered via <c>RegisterEntryPoint</c>, so <c>IStartable.Start</c> runs <see cref="Load"/>
    /// automatically at scope start. <see cref="Save"/> is never called automatically — call it from an
    /// options UI (on Apply/Close) or on <c>OnApplicationQuit</c>.
    ///
    /// Duplicate <c>Key</c> values across two settings throw at container construction
    /// (<c>ToDictionary</c>). That is intentional — keys must be unique.
    /// </remarks>
    public class SettingsService : ISettingsService, IStartable
    {
        private readonly Dictionary<string, ISettings> m_Settings;
        private readonly string m_FilePath;

        [Inject]
        public SettingsService(IEnumerable<ISettings> settings)
        {
            m_FilePath = Path.Combine(Application.persistentDataPath, EnvironmentVariables.SettingsFileName);
            m_Settings = settings.ToDictionary(s => s.Key);
        }

        void IStartable.Start() => Load();

        public ISettings<T> Get<T>(string key) => (ISettings<T>)m_Settings[key];

        public ISettings Get(string key) => m_Settings[key];

        public IEnumerable<ISettings> GetAll() => m_Settings.Values;

        public void Save()
        {
            IEnumerable<KeyValuePair<string, string>> data = m_Settings
                .Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value.Serialize()));

            string content = CfgParser.Serialize("Settings Configuration File", data);
            string tempPath = m_FilePath + ".tmp";

            // Atomic write pattern: write to a temp file first, then replace.
            // This prevents data corruption if a crash occurs mid-write.
            // File.Replace also creates a backup (.bak) of the previous file.

            File.WriteAllText(tempPath, content);

            if (File.Exists(m_FilePath))
                File.Replace(tempPath, m_FilePath, m_FilePath + ".bak");
            else
                File.Move(tempPath, m_FilePath);
        }

        public void Load()
        {
            if (File.Exists(m_FilePath))
            {
                string content = File.ReadAllText(m_FilePath);
                Dictionary<string, string> data = CfgParser.Parse(content);

                foreach (KeyValuePair<string, string> kvp in data)
                {
                    if (m_Settings.TryGetValue(kvp.Key, out ISettings setting))
                        setting.Load(kvp.Value);
                }
            }

            // Apply all settings after loading (covers settings absent from the file, which keep defaults).
            // Known limitation: dictionary iteration order is not guaranteed, so vsync / target framerate /
            // quality level can overwrite each other. Add an int Order to ISettings and sort here if
            // deterministic apply order matters for your project.
            foreach (ISettings setting in m_Settings.Values)
                setting.Apply();
        }

        public void ResetAll()
        {
            foreach (ISettings setting in m_Settings.Values)
                setting.Reset();
        }
    }
}
