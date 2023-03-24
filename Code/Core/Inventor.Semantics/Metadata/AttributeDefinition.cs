using System;

namespace Inventor.Semantics.Metadata
{
	public class AttributeJsonSerializationSettings : IAttributeSerializationSettings, IJsonSerializationSettings
	{
		public String JsonElementName
		{ get; }

		public Type JsonType
		{ get { return null; } }

		public AttributeJsonSerializationSettings(
			Serialization.Xml.Attribute xml)
		{
			if (xml == null) throw new ArgumentNullException(nameof(xml));

			JsonElementName = xml.GetType().Name.Replace("Attribute", "");
		}
	}

	public class AttributeXmlSerializationSettings : IAttributeSerializationSettings, IXmlSerializationSettings
	{
		public Serialization.Xml.Attribute Xml
		{ get; }

		public String XmlElementName
		{ get; }

		public Type XmlType
		{ get; }

		public AttributeXmlSerializationSettings(
			Serialization.Xml.Attribute xml)
		{
			if (xml == null) throw new ArgumentNullException(nameof(xml));

			Xml = xml;
			XmlType = xml.GetType();
			XmlElementName = XmlType.Name.Replace("Attribute", "");
		}
	}

	public class AttributeDefinition : MetadataDefinition<AttributeJsonSerializationSettings, AttributeXmlSerializationSettings>
	{
		#region Properties

		public IAttribute AttributeValue
		{ get; }

		private readonly Func<ILanguage, String> _attributeNameGetter;

		#endregion

		#region Constructors

		public AttributeDefinition(
			Type type,
			IAttribute attributeValue,
			Func<ILanguage, String> attributeNameGetter,
			Serialization.Xml.Attribute xml)
			: base(
				type,
				typeof(IAttribute),
				new AttributeJsonSerializationSettings(xml),
				new AttributeXmlSerializationSettings(xml))
		{
			if (attributeValue == null) throw new ArgumentNullException(nameof(attributeValue));
			if (!type.IsInstanceOfType(attributeValue)) throw new InvalidCastException();
			if (attributeNameGetter == null) throw new ArgumentNullException(nameof(attributeNameGetter));

			AttributeValue = attributeValue;
			_attributeNameGetter = attributeNameGetter;
		}

		private AttributeDefinition()
			: base(
				typeof(NoAttribute),
				typeof(IAttribute),
				new AttributeJsonSerializationSettings(new Modules.Boolean.Xml.IsValueAttribute()),
				new AttributeXmlSerializationSettings(new Modules.Boolean.Xml.IsValueAttribute()))
		{
			AttributeValue = new NoAttribute();
			_attributeNameGetter = language => language.Attributes.None;
		}

		#endregion

		public String GetName(ILanguage language)
		{
			return _attributeNameGetter(language);
		}

		public static readonly AttributeDefinition None = new AttributeDefinition();

		private class NoAttribute : IAttribute
		{ }
	}

	public class AttributeDefinition<AttributeT> : AttributeDefinition
		where AttributeT : IAttribute
	{
		public AttributeDefinition(
			Func<ILanguage, String> attributeNameGetter,
			AttributeT attributeValue,
			Serialization.Xml.Attribute xml)
			: base(
				typeof(AttributeT),
				attributeValue,
				attributeNameGetter,
				xml)
		{
			if (attributeNameGetter == null) throw new ArgumentNullException(nameof(attributeNameGetter));
		}
	}

	public class AttributeDefinition<AttributeT, XmlT> : AttributeDefinition<AttributeT>
		where AttributeT : IAttribute
		where XmlT : Serialization.Xml.Attribute
	{
		public AttributeDefinition(
			Func<ILanguage, String> attributeNameGetter,
			AttributeT attributeValue,
			XmlT xml)
			: base(
				attributeNameGetter,
				attributeValue,
				xml)
		{
			if (attributeNameGetter == null) throw new ArgumentNullException(nameof(attributeNameGetter));
		}
	}
}