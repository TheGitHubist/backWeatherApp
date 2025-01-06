using System;
using System.Collections.Generic;
using System.IO;

namespace Save {
    public class SaveFile {
        private const string FileName = "options.json";
        private Dictionary<string, string> options = new Dictionary<string, string>();
        public SaveFile() {
            options.Add("city", "London");
            options.Add("language", "en");
            File.WriteAllText(FileName, options.ToString());
        }
        public void Save(string? city, string? language)
        {
            if (city != null)
            {
                options["city"] = city;
            }
            if (language != null)
            {
                options["language"] = language;
            }
            if (city == null && language == null)
            {
                return;
            }
            File.WriteAllText(FileName, options.ToString());
        }
    }
}