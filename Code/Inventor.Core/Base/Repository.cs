using System;
using System.Collections.Generic;

namespace Inventor.Core.Base
{
	public class Repository<DefinitionT>: IRepository<DefinitionT>
		where DefinitionT : IMetadataDefinition
	{
		public IDictionary<Type, DefinitionT> Definitions
		{ get; } = new Dictionary<Type, DefinitionT>();
	}
}
