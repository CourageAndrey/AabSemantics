using System;
using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IRepository<DefinitionT>
		where DefinitionT : IMetadataDefinition
	{
		IDictionary<Type, DefinitionT> Definitions
		{ get; }
	}

	public static class RepositoryExtensions
	{
		public static void Define<DefinitionT>(this IRepository<DefinitionT> repository, DefinitionT questionDefinition)
			where DefinitionT : IMetadataDefinition
		{
			repository.Definitions[questionDefinition.Type] = questionDefinition;
		}
	}
}
