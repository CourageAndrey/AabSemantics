using System;
using System.Collections.Generic;

namespace Inventor.Core.Base
{
	public class StatementRepository : IStatementRepository
	{
		public IDictionary<Type, StatementDefinition> Definitions
		{ get; } = new Dictionary<Type, StatementDefinition>();

		public void Define(StatementDefinition statementDefinition)
		{
			Definitions[statementDefinition.Type] = statementDefinition;
		}
	}
}
