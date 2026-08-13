using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Metadata;

namespace AabSemantics
{
	/// <summary>
	/// A registry of metadata definitions, keyed by the type each one describes.
	/// </summary>
	/// <typeparam name="DefinitionT">Kind of definition stored, e.g. statement or question metadata.</typeparam>
	public interface IMetadataRepository<DefinitionT>
		where DefinitionT : IMetadataDefinition
	{
		/// <summary>
		/// Registered definitions, keyed by described type.
		/// </summary>
		IDictionary<Type, DefinitionT> Definitions
		{ get; }
	}

	/// <summary>
	/// Default in-memory <see cref="IMetadataRepository{DefinitionT}"/>.
	/// </summary>
	/// <typeparam name="DefinitionT">Kind of definition stored.</typeparam>
	public class MetadataRepository<DefinitionT> : IMetadataRepository<DefinitionT>
		where DefinitionT : IMetadataDefinition
	{
		/// <summary>
		/// Registered definitions, keyed by described type.
		/// </summary>
		public IDictionary<Type, DefinitionT> Definitions
		{ get; } = new Dictionary<Type, DefinitionT>();
	}

	/// <summary>
	/// Lookups across a metadata registry.
	/// </summary>
	public static class MetadataRepositoryExtensions
	{
		/// <summary>
		/// Finds the definition describing an instance, walking up its base types until one
		/// matches. This lets a module register a single definition for a whole hierarchy.
		/// </summary>
		/// <typeparam name="InstanceT">Static type of the instance.</typeparam>
		/// <typeparam name="DefinitionT">Kind of definition stored.</typeparam>
		/// <param name="repository">Registry to search.</param>
		/// <param name="instance">Instance whose definition is wanted; matched by runtime type.</param>
		/// <returns>The nearest matching definition.</returns>
		/// <exception cref="NotSupportedException">Neither the type nor any of its base types is registered.</exception>
		public static DefinitionT GetSuitable<InstanceT, DefinitionT>(this IDictionary<Type, DefinitionT> repository, InstanceT instance)
		{
			var type = instance.GetType();

			while (type != null)
			{
				DefinitionT definition;
				if (repository.TryGetValue(type, out definition))
				{
					return definition;
				}

				type = type.BaseType;
			}

			throw new NotSupportedException();
		}

		/// <summary>
		/// Collects the JSON surrogate types of every registered definition, which is what the
		/// serializer needs in order to resolve polymorphic payloads.
		/// </summary>
		/// <typeparam name="DefinitionT">Kind of definition stored.</typeparam>
		/// <param name="repository">Registry to read.</param>
		/// <returns>One JSON type per registered definition.</returns>
		public static List<Type> GetJsonTypes<DefinitionT>(this IMetadataRepository<DefinitionT> repository)
			where DefinitionT : IMetadataDefinition
		{
			return repository.Definitions.Values.Select(definition => definition.GetSerializationSettings<IJsonSerializationSettings>().JsonType).ToList();
		}
	}
}
