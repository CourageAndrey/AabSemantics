using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Semantics
{
	public interface IRepository<DefinitionT>
		where DefinitionT : IMetadataDefinition
	{
		IDictionary<Type, DefinitionT> Definitions
		{ get; }
	}

	public class Repository<DefinitionT> : IRepository<DefinitionT>
		where DefinitionT : IMetadataDefinition
	{
		public IDictionary<Type, DefinitionT> Definitions
		{ get; } = new Dictionary<Type, DefinitionT>();
	}

	public static class RepositoryExtensions
	{
		public static void Define<DefinitionT>(this IRepository<DefinitionT> repository, DefinitionT questionDefinition)
			where DefinitionT : IMetadataDefinition
		{
			repository.Definitions[questionDefinition.Type] = questionDefinition;
		}

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

		public static List<Type> GetJsonTypes<DefinitionT>(this IRepository<DefinitionT> repository)
			where DefinitionT : IMetadataDefinition
		{
			return repository.Definitions.Values.Select(definition => definition.JsonSerializationSettings.JsonType).ToList();
		}
	}
}
