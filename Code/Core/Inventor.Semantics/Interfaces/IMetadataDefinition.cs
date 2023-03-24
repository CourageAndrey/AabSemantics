using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Metadata;

namespace Inventor.Semantics
{
	public interface IMetadataDefinition
	{
		Type Type
		{ get; }

		List<ISerializationSettings> SerializationSettings
		{ get; }
	}

	public interface IMetadataDefinition<SerializationSettingsT> : IMetadataDefinition
		where SerializationSettingsT : ISerializationSettings
	{
		new List<SerializationSettingsT> SerializationSettings
		{ get; }
	}

	public abstract class MetadataDefinition<SerializationSettingsT> : IMetadataDefinition<SerializationSettingsT>
		where SerializationSettingsT : ISerializationSettings
	{
		#region Properties

		public Type Type
		{ get; }

		List<ISerializationSettings> IMetadataDefinition.SerializationSettings
		{ get { return SerializationSettings.OfType<ISerializationSettings>().ToList(); } }

		public List<SerializationSettingsT> SerializationSettings
		{ get; } = new List<SerializationSettingsT>();

		#endregion

		protected MetadataDefinition(Type type, Type instanceType)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (type.IsAbstract || !instanceType.IsAssignableFrom(type)) throw new ArgumentException($"Type must be non-abstract and implement {instanceType}.", nameof(type));

			Type = type;
		}
	}

	public static class MetadataDefinitionExtensions
	{
		public static SettingsT GetSerializationSettings<SettingsT>(this IMetadataDefinition metadataDefinition)
			where SettingsT : ISerializationSettings
		{
			return metadataDefinition.SerializationSettings.OfType<SettingsT>().Single();
		}
	}
}
