using System;

using Inventor.Semantics.Metadata;

namespace Inventor.Semantics
{
	public interface IMetadataDefinition
	{
		Type Type
		{ get; }

		IJsonSerializationSettings JsonSerializationSettings
		{ get; }

		IXmlSerializationSettings XmlSerializationSettings
		{ get; }
	}

	public interface IMetadataDefinition<out JsonSerializationSettingsT, out XmlSerializationSettingsT> : IMetadataDefinition
		where JsonSerializationSettingsT : ISerializationSettings, IJsonSerializationSettings
		where XmlSerializationSettingsT : ISerializationSettings, IXmlSerializationSettings
	{
		new JsonSerializationSettingsT JsonSerializationSettings
		{ get; }

		new XmlSerializationSettingsT XmlSerializationSettings
		{ get; }
	}

	public abstract class MetadataDefinition<JsonSerializationSettingsT, XmlSerializationSettingsT> : IMetadataDefinition<JsonSerializationSettingsT, XmlSerializationSettingsT>
		where JsonSerializationSettingsT : IJsonSerializationSettings
		where XmlSerializationSettingsT : IXmlSerializationSettings
	{
		#region Properties

		public Type Type
		{ get; }

		public JsonSerializationSettingsT JsonSerializationSettings
		{ get; }

		public XmlSerializationSettingsT XmlSerializationSettings
		{ get; }

		IJsonSerializationSettings IMetadataDefinition.JsonSerializationSettings
		{ get { return JsonSerializationSettings; } }

		IXmlSerializationSettings IMetadataDefinition.XmlSerializationSettings
		{ get { return XmlSerializationSettings; } }

		#endregion

		protected MetadataDefinition(Type type, Type instanceType, JsonSerializationSettingsT jsonSerializationSettings, XmlSerializationSettingsT xmlSerializationSettings)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (type.IsAbstract || !instanceType.IsAssignableFrom(type)) throw new ArgumentException($"Type must be non-abstract and implement {instanceType}.", nameof(type));
			if (jsonSerializationSettings == null) throw new ArgumentNullException(nameof(jsonSerializationSettings));
			if (xmlSerializationSettings == null) throw new ArgumentNullException(nameof(xmlSerializationSettings));

			Type = type;
			JsonSerializationSettings = jsonSerializationSettings;
			XmlSerializationSettings = xmlSerializationSettings;
		}
	}
}
