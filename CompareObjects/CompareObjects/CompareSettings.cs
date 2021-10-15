using System.Collections.Generic;

namespace CompareObjects
{
    public class CompareSettings
    {
        public bool CaseSensitive { get; set; }
        public bool CompareChildren { get; set; }
        public string CustomDefaultText { get; set; }
        public Dictionary<string, string> DefaultPropertyTexts { get; set; }
        public List<string> IgnoredFields { get; set; }
        public Dictionary<string, string> PropertiesDisplayNames { get; set; }
    }
}
