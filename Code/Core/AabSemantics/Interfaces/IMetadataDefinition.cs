using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Metadata;
using AabSemantics.Utils;

namespace AabSemantics
{
	/// <summary>
	/// Runtime description of one extensible type — a statement, question, answer or attribute kind.
	/// Modules register these so the engine can serialize, render and dispatch on types it was
	/// not compiled against.
	/// </summary>
	public interface IMetadataDefinition
	{
		/// <summary>
		/// The described type.
		/// </summary>
		Type Type
		{ get; }

		/// <summary>
		/// How instances of the type are persisted, one entry per supported format.
		/// </summary>
		List<ISerializationSettings> SerializationSettings
		{ get; }
	}

	/// <summary>
	/// A metadata definition whose serialization settings are exposed in their concrete type.
	/// </summary>
	/// <typeparam name="SerializationSettingsT">Concrete settings type.</typeparam>
	public interface IMetadataDefinition<SerializationSettingsT> : IMetadataDefinition
		where SerializationSettingsT : ISerializationSettings
	{
		/// <summary>
		/// How instances of the type are persisted, strongly typed.
		/// </summary>
		new List<SerializationSettingsT> SerializationSettings
		{ get; }
	}

	/// <summary>
	/// Base class for metadata definitions. It verifies at construction that the described type
	/// really implements the contract it is being registered for, turning a wiring mistake into
	/// an immediate failure rather than a later cast error.
	/// </summary>
	/// <typeparam name="SerializationSettingsT">Concrete settings type.</typeparam>
	public abstract class MetadataDefinition<SerializationSettingsT> : IMetadataDefinition<SerializationSettingsT>
		where SerializationSettingsT : ISerializationSettings
	{
		#region Properties

		/// <summary>
		/// The described type.
		/// </summary>
		public Type Type
		{ get; }

		List<ISerializationSettings> IMetadataDefinition.SerializationSettings
		{ get { return SerializationSettings.OfType<ISerializationSettings>().ToList(); } }

		/// <summary>
		/// How instances of the type are persisted, one entry per supported format.
		/// </summary>
		public List<SerializationSettingsT> SerializationSettings
		{ get; } = new List<SerializationSettingsT>();

		#endregion

		/// <summary>
		/// Initializes the definition.
		/// </summary>
		/// <param name="type">Type being described.</param>
		/// <param name="instanceType">Contract <paramref name="type"/> is required to satisfy.</param>
		/// <exception cref="ArgumentNullException"><paramref name="type"/> is <c>null</c>.</exception>
		protected MetadataDefinition(Type type, Type instanceType)
		{
			Type = type.EnsureNotNull(nameof(type));
			type.EnsureContract(instanceType, nameof(type));
		}
	}

	/// <summary>
	/// Helpers for reading serialization settings off a metadata definition.
	/// </summary>
	public static class MetadataDefinitionExtensions
	{
		/// <summary>
		/// Returns the definition's settings for one serialization format.
		/// </summary>
		/// <typeparam name="SettingsT">Settings type identifying the format.</typeparam>
		/// <param name="metadataDefinition">Definition to read.</param>
		/// <returns>The matching settings.</returns>
		/// <exception cref="InvalidOperationException">The format is not configured, or configured more than once.</exception>
		public static SettingsT GetSerializationSettings<SettingsT>(this IMetadataDefinition metadataDefinition)
			where SettingsT : ISerializationSettings
		{
			return metadataDefinition.SerializationSettings.OfType<SettingsT>().Single();
		}
	}
}
