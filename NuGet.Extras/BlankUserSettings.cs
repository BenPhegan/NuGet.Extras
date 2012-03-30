using System.Collections.Generic;

namespace NuGet.Extras
{
    public class BlankUserSettings : ISettings
    {
        public bool DeleteSection(string section)
        {
            return true;
        }

        public bool DeleteValue(string section, string key)
        {
            return true;
        }

        public string GetValue(string section, string key)
        {
            return null;
        }

        public IList<KeyValuePair<string, string>> GetValues(string section)
        {
            return null;
        }

        public void SetValue(string section, string key, string value)
        {
        }

        public void SetValues(string section, IList<KeyValuePair<string, string>> values)
        {
        }

        public IList<KeyValuePair<string, string>> GetNestedValues(string section, string key)
        {
            return new List<KeyValuePair<string, string>>();
        }

        public void SetNestedValues(string section, string key, IList<KeyValuePair<string, string>> values)
        {
        }
    }
}
