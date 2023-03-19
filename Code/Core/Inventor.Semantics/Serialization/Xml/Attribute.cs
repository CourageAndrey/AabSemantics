using System.Xml.Serialization;

using Inventor.Semantics.Metadata;

namespace Inventor.Semantics.Serialization.Xml
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

	[XmlType]
	public abstract class Attribute<AttributeT> : Attribute
		where AttributeT : IAttribute
	{
		public override IAttribute Load()
		{
			return LoadTyped();
		}

		public abstract AttributeT LoadTyped();
	}
}