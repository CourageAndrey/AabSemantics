using System;
using System.Xml.Serialization;

namespace Sef.Program
{
    [Serializable, XmlRoot]
    public class Configuration
    {
        [XmlElement]
        public String SelectedLanguage
        { get; set; }

        [XmlIgnore]
        internal const string FileName = "Configuration.xml";
    }
}
