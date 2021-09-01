using System;
using System.Collections.Generic;

namespace Inventor.Core.Base
{
	public class StatementRepository : IStatementRepository
	{
		public IDictionary<Type, StatementDefinition> StatementDefinitions
		{ get; } = new Dictionary<Type, StatementDefinition>();

		public void DefineStatement(StatementDefinition statementDefinition)
		{
			StatementDefinitions[statementDefinition.StatementType] = statementDefinition;
		}
	}
}
