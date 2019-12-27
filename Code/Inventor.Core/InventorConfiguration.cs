using System.Xml.Serialization;

using Sef.Interfaces;
using Sef.Program;

namespace Inventor.Core
{
    [XmlRoot]
    public class InventorConfiguration : Configuration, IEditable<InventorConfiguration>
    {
        [XmlElement]
        public bool IncludeExplaining
        { get; set; }
#warning Реализовать этот флаг

        [XmlElement]
        public bool AutoValidate
        { get; set; }
#warning Реализовать этот флаг

        public void UpdateFrom(InventorConfiguration other)
        {
            SelectedLanguage = other.SelectedLanguage;
            IncludeExplaining = other.IncludeExplaining;
            AutoValidate = other.AutoValidate;
        }
    }
}
