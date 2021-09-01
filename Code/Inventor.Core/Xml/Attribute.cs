using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[XmlType]
	public abstract class Attribute
	{
		public abstract IAttribute Load();

		public static Attribute Save(IAttribute attribute)
		{
			var definition = Repositories.Attributes.Definitions.GetSuitable(attribute);
			return definition.Xml;
		}
	}
}