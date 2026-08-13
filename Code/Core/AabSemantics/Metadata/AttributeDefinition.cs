using System;

using AabSemantics.Utils;

namespace AabSemantics.Metadata
{
	/// <summary>
	/// JSON persistence settings for an attribute. Attributes carry no state, so they are written
	/// as a bare element name rather than as an object.
	/// </summary>
	public class AttributeJsonSerializationSettings : IAttributeSerializationSettings, IJsonSerializationSettings
	{
		/// <summary>
		/// Element name written to JSON: the XML surrogate's type name without its "Attribute" suffix.
		/// </summary>
		public String JsonElementName
		{ get; }

		/// <summary>
		/// Always <c>null</c>: an attribute has no JSON surrogate type of its own.
		/// </summary>
		public Type JsonType
		{ get { return null; } }

		/// <summary>
		/// Derives the JSON element name from the attribute's XML surrogate.
		/// </summary>
		/// <param name="xml">XML surrogate instance; must not be <c>null</c>.</param>
		public AttributeJsonSerializationSettings(
			Serialization.Xml.Attribute xml)
		{
			JsonElementName = xml.EnsureNotNull(nameof(xml)).GetType().Name.Replace("Attribute", "");
		}
	}

	/// <summary>
	/// XML persistence settings for an attribute.
	/// </summary>
	public class AttributeXmlSerializationSettings : IAttributeSerializationSettings, IXmlSerializationSettings
	{
		/// <summary>
		/// The surrogate instance written in place of the attribute.
		/// </summary>
		public Serialization.Xml.Attribute Xml
		{ get; }

		/// <summary>
		/// Element name written to XML: the surrogate's type name without its "Attribute" suffix.
		/// </summary>
		public String XmlElementName
		{ get; }

		/// <summary>
		/// Type of the XML surrogate.
		/// </summary>
		public Type XmlType
		{ get; }

		/// <summary>
		/// Derives the XML settings from the attribute's surrogate instance.
		/// </summary>
		/// <param name="xml">XML surrogate instance; must not be <c>null</c>.</param>
		public AttributeXmlSerializationSettings(
			Serialization.Xml.Attribute xml)
		{
			Xml = xml.EnsureNotNull(nameof(xml));
			XmlType = xml.GetType();
			XmlElementName = XmlType.Name.Replace("Attribute", "");
		}
	}

	/// <summary>
	/// Runtime description of an attribute kind: the single instance representing it and
	/// how to name it in each language.
	/// </summary>
	public class AttributeDefinition : MetadataDefinition<IAttributeSerializationSettings>
	{
		#region Properties

		/// <summary>
		/// The attribute instance itself. Attributes are stateless, so one instance is shared
		/// by every concept carrying it.
		/// </summary>
		public IAttribute Value
		{ get; }

		private readonly Func<ILanguage, String> _nameGetter;

		#endregion

		#region Constructors

		/// <summary>
		/// Describes an attribute kind.
		/// </summary>
		/// <param name="type">Attribute type being described.</param>
		/// <param name="value">The shared instance of that type.</param>
		/// <param name="nameGetter">Selects the attribute's display name from a language.</param>
		/// <exception cref="InvalidCastException"><paramref name="value"/> is not an instance of <paramref name="type"/>.</exception>
		public AttributeDefinition(
			Type type,
			IAttribute value,
			Func<ILanguage, String> nameGetter)
			: base(type, typeof(IAttribute))
		{
			Value = value.EnsureNotNull(nameof(value));
			if (!type.IsInstanceOfType(value)) throw new InvalidCastException();
			_nameGetter = nameGetter.EnsureNotNull(nameof(nameGetter));
		}

		private AttributeDefinition()
			: base(typeof(NoAttribute), typeof(IAttribute))
		{
			Value = new NoAttribute();
			_nameGetter = language => language.Attributes.None;
		}

		#endregion

		/// <summary>
		/// Returns the attribute's display name in a language.
		/// </summary>
		/// <param name="language">Language to read the name in.</param>
		/// <returns>The localized name.</returns>
		public String GetName(ILanguage language)
		{
			return _nameGetter(language);
		}

		/// <summary>
		/// Placeholder definition standing for "no attribute", used where a definition is
		/// required but no attribute applies.
		/// </summary>
		public static readonly AttributeDefinition None = new AttributeDefinition();

		private class NoAttribute : IAttribute
		{ }
	}

	/// <summary>
	/// Strongly typed <see cref="AttributeDefinition"/>, sparing the caller an explicit type argument.
	/// </summary>
	/// <typeparam name="AttributeT">Attribute type being described.</typeparam>
	public class AttributeDefinition<AttributeT> : AttributeDefinition
		where AttributeT : IAttribute
	{
		/// <summary>
		/// Describes an attribute kind.
		/// </summary>
		/// <param name="value">The shared instance of the attribute type.</param>
		/// <param name="nameGetter">Selects the attribute's display name from a language.</param>
		public AttributeDefinition(
			AttributeT value,
			Func<ILanguage, String> nameGetter)
			: base(typeof(AttributeT), value, nameGetter)
		{ }
	}

	/// <summary>
	/// Fluent configuration of how an attribute is persisted.
	/// </summary>
	public static class AttributeDefinitionExtensions
	{
		/// <summary>
		/// Returns the definition's XML settings.
		/// </summary>
		/// <param name="metadataDefinition">Definition to read.</param>
		/// <returns>The XML settings.</returns>
		/// <exception cref="InvalidOperationException">XML serialization has not been configured.</exception>
		public static IXmlSerializationSettings GetXmlSerializationSettings(this AttributeDefinition metadataDefinition)
		{
			return metadataDefinition.GetSerializationSettings<AttributeXmlSerializationSettings>();
		}

		/// <summary>
		/// Returns the definition's JSON settings.
		/// </summary>
		/// <param name="metadataDefinition">Definition to read.</param>
		/// <returns>The JSON settings.</returns>
		/// <exception cref="InvalidOperationException">JSON serialization has not been configured.</exception>
		public static IJsonSerializationSettings GetJsonSerializationSettings(this AttributeDefinition metadataDefinition)
		{
			return metadataDefinition.GetSerializationSettings<AttributeJsonSerializationSettings>();
		}

		/// <summary>
		/// Configures XML persistence for the attribute.
		/// </summary>
		/// <param name="metadataDefinition">Definition to configure.</param>
		/// <param name="xml">Surrogate instance written in place of the attribute.</param>
		/// <returns>The same definition, to allow call chaining.</returns>
		public static AttributeDefinition SerializeToXml(
			this AttributeDefinition metadataDefinition,
			Serialization.Xml.Attribute xml)
		{
			metadataDefinition.SerializationSettings.Add(new AttributeXmlSerializationSettings(xml));
			return metadataDefinition;
		}

		/// <summary>
		/// Configures JSON persistence for the attribute. The XML surrogate is reused here
		/// purely to derive the element name.
		/// </summary>
		/// <param name="metadataDefinition">Definition to configure.</param>
		/// <param name="xml">Surrogate instance the element name is derived from.</param>
		/// <returns>The same definition, to allow call chaining.</returns>
		public static AttributeDefinition SerializeToJson(
			this AttributeDefinition metadataDefinition,
			Serialization.Xml.Attribute xml)
		{
			metadataDefinition.SerializationSettings.Add(new AttributeJsonSerializationSettings(xml));
			return metadataDefinition;
		}
	}
}