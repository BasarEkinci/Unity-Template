using System.Collections.Generic;

namespace Syntac.SettingsSystem.Core
{
    public interface ISettingsService
    {
        ISettings<T> Get<T>(string key);
        ISettings Get(string key);
        IEnumerable<ISettings> GetAll();
        void Save();
        void Load();
        void ResetAll();
    }
}
