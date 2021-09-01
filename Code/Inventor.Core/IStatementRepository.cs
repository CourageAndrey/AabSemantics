using System;
using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IStatementRepository
	{
		IDictionary<Type, StatementDefinition> Definitions
		{ get; }

		void Define(StatementDefinition statementDefinition);
	}
}
