using System;
using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IRepository<DefinitionT>
		where DefinitionT : IMetadataDefinition
	{
		IDictionary<Type, DefinitionT> Definitions
		{ get; }

		void Define(DefinitionT questionDefinition);
	}
}
