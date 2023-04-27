using System;

using Inventor.Semantics.Utils;

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
			JsonElementName = xml.EnsureNotNull(nameof(xml)).GetType().Name.Replace("Attribute", "");
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
			Xml = xml.EnsureNotNull(nameof(xml));
			XmlType = xml.GetType();
			XmlElementName = XmlType.Name.Replace("Attribute", "");
		}
	}

	public class AttributeDefinition : MetadataDefinition<IAttributeSerializationSettings>
	{
		#region Properties

		public IAttribute Value
		{ get; }

		private readonly Func<ILanguage, String> _nameGetter;

		#endregion

		#region Constructors

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

		public String GetName(ILanguage language)
		{
			return _nameGetter(language);
		}

		public static readonly AttributeDefinition None = new AttributeDefinition();

		private class NoAttribute : IAttribute
		{ }
	}

	public class AttributeDefinition<AttributeT> : AttributeDefinition
		where AttributeT : IAttribute
	{
		public AttributeDefinition(
			AttributeT value,
			Func<ILanguage, String> nameGetter)
			: base(typeof(AttributeT), value, nameGetter)
		{ }
	}

	public static class AttributeDefinitionExtensions
	{
		public static IXmlSerializationSettings GetXmlSerializationSettings(this AttributeDefinition metadataDefinition)
		{
			return metadataDefinition.GetXmlSerializationSettings<AttributeXmlSerializationSettings>();
		}

		public static SettingsT GetXmlSerializationSettings<SettingsT>(this AttributeDefinition metadataDefinition)
			where SettingsT : IXmlSerializationSettings, IAttributeSerializationSettings
		{
			return metadataDefinition.GetSerializationSettings<SettingsT>();
		}

		public static IJsonSerializationSettings GetJsonSerializationSettings(this AttributeDefinition metadataDefinition)
		{
			return metadataDefinition.GetJsonSerializationSettings<AttributeJsonSerializationSettings>();
		}

		public static SettingsT GetJsonSerializationSettings<SettingsT>(this AttributeDefinition metadataDefinition)
			where SettingsT : IJsonSerializationSettings, IAttributeSerializationSettings
		{
			return metadataDefinition.GetSerializationSettings<SettingsT>();
		}

		public static AttributeDefinition SerializeToXml(
			this AttributeDefinition metadataDefinition,
			Serialization.Xml.Attribute xml)
		{
			metadataDefinition.SerializationSettings.Add(new AttributeXmlSerializationSettings(xml));
			return metadataDefinition;
		}

		public static AttributeDefinition SerializeToJson(
			this AttributeDefinition metadataDefinition,
			Serialization.Xml.Attribute xml)
		{
			metadataDefinition.SerializationSettings.Add(new AttributeJsonSerializationSettings(xml));
			return metadataDefinition;
		}
	}
}