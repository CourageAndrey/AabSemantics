using System.Xml.Serialization;

using Inventor.Semantics.Metadata;

namespace Inventor.Semantics.Xml
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