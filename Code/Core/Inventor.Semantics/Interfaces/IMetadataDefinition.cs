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
}
