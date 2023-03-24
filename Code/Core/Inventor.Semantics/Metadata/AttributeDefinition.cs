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

	public class AttributeDefinition : IMetadataDefinition<AttributeJsonSerializationSettings, AttributeXmlSerializationSettings>
	{
		#region Properties

		public Type Type
		{ get; }

		public IAttribute AttributeValue
		{ get; }

		public AttributeJsonSerializationSettings JsonSerializationSettings
		{ get; }

		public AttributeXmlSerializationSettings XmlSerializationSettings
		{ get; }

		IJsonSerializationSettings IMetadataDefinition.JsonSerializationSettings
		{ get { return JsonSerializationSettings; } }

		IXmlSerializationSettings IMetadataDefinition.XmlSerializationSettings
		{ get { return XmlSerializationSettings; } }

		private readonly Func<ILanguage, String> _attributeNameGetter;

		#endregion

		#region Constructors

		public AttributeDefinition(
			Type type,
			IAttribute attributeValue,
			Func<ILanguage, String> attributeNameGetter,
			Serialization.Xml.Attribute xml)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (type.IsAbstract || !typeof(IAttribute).IsAssignableFrom(type)) throw new ArgumentException($"Type must be non-abstract and implement {typeof(IAttribute)}.", nameof(type));
			if (attributeValue == null) throw new ArgumentNullException(nameof(attributeValue));
			if (!type.IsInstanceOfType(attributeValue)) throw new InvalidCastException();
			if (attributeNameGetter == null) throw new ArgumentNullException(nameof(attributeNameGetter));

			Type = type;
			AttributeValue = attributeValue;
			_attributeNameGetter = attributeNameGetter;
			JsonSerializationSettings = new AttributeJsonSerializationSettings(xml);
			XmlSerializationSettings = new AttributeXmlSerializationSettings(xml);
		}

		private AttributeDefinition()
		{
			Type = typeof(NoAttribute);
			AttributeValue = new NoAttribute();
			_attributeNameGetter = language => language.Attributes.None;
			JsonSerializationSettings = null;
			XmlSerializationSettings = null;
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