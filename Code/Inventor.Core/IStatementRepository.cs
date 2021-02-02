using System;
using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IStatementRepository
	{
		IDictionary<Type, StatementDefinition> StatementDefinitions
		{ get; }

		void DefineStatement(StatementDefinition statementDefinition);
	}
}
