using System;
using System.Collections.Generic;

namespace Inventor.Core.Base
{
	public class StatementRepository : IStatementRepository
	{
		public IDictionary<Type, StatementDefinition> StatementDefinitions
		{ get; }

		public StatementRepository()
		{
			StatementDefinitions = new Dictionary<Type, StatementDefinition>();
			DefineStatement(new StatementDefinition(typeof(Statements.HasPartStatement), language => language.StatementNames.Composition));
			DefineStatement(new StatementDefinition(typeof(Statements.GroupStatement), language => language.StatementNames.SubjectArea));
			DefineStatement(new StatementDefinition(typeof(Statements.HasSignStatement), language => language.StatementNames.HasSign));
			DefineStatement(new StatementDefinition(typeof(Statements.IsStatement), language => language.StatementNames.Clasification));
			DefineStatement(new StatementDefinition(typeof(Statements.SignValueStatement), language => language.StatementNames.SignValue));
			DefineStatement(new StatementDefinition(typeof(Statements.ComparisonStatement), language => language.StatementNames.Comparison));
			DefineStatement(new StatementDefinition(typeof(Statements.ProcessesStatement), language => language.StatementNames.Processes));
		}

		public void DefineStatement(StatementDefinition statementDefinition)
		{
			StatementDefinitions[statementDefinition.StatementType] = statementDefinition;
		}
	}
}
