using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[XmlType]
	public abstract class Attribute
	{
		public abstract IAttribute Load();

		public static Attribute Save(IAttribute attribute, IAttributeRepository repository)
		{
			var definition = repository.AttributeDefinitions.GetSuitable(attribute);
			return definition.Xml;
		}
	}
}