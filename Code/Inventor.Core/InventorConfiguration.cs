using System.Xml.Serialization;

using Sef.Interfaces;
using Sef.Program;

namespace Inventor.Core
{
    [XmlRoot]
    public class InventorConfiguration : Configuration, IEditable<InventorConfiguration>
    {
        public void UpdateFrom(InventorConfiguration other)
        {
            SelectedLanguage = other.SelectedLanguage;
        }
    }
}
