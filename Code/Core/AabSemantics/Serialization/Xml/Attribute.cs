using System.Xml.Serialization;

using AabSemantics.Metadata;

namespace AabSemantics.Serialization.Xml
{
	[XmlType]
	public abstract class Attribute
	{
		public abstract IAttribute Load();

		public static Attribute Save(IAttribute attribute)
		{
			var definition = Repositories.Attributes.Definitions.GetSuitable(attribute);
			return definition.GetSerializationSettings<AttributeXmlSerializationSettings>().Xml;
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