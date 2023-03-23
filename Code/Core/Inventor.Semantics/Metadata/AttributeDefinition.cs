using System;

namespace Inventor.Semantics.Metadata
{
	public class AttributeDefinition : IMetadataDefinition
	{
		#region Properties

		public Type Type
		{ get; }

		public IAttribute AttributeValue
		{ get; }

		public Serialization.Xml.Attribute Xml
		{ get; }

		public String XmlElementName
		{ get; }

		public String JsonElementName
		{ get; }

		public Type XmlType
		{ get; }

		public Type JsonType
		{ get { return null; } }

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
			if (xml == null) throw new ArgumentNullException(nameof(xml));

			Type = type;
			AttributeValue = attributeValue;
			_attributeNameGetter = attributeNameGetter;
			Xml = xml;
			XmlType = xml.GetType();
			JsonElementName = XmlElementName = XmlType.Name.Replace("Attribute", "");
		}

		private AttributeDefinition()
		{
			Type = typeof(NoAttribute);
			AttributeValue = new NoAttribute();
			_attributeNameGetter = language => language.Attributes.None;
			Xml = null;
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