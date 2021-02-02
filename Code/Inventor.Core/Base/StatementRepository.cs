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
			DefineStatement(new StatementDefinition(typeof(Statements.IsNotEqualToStatement), language => language.StatementNames.IsNotEqualTo));
			DefineStatement(new StatementDefinition(typeof(Statements.IsLessThanStatement), language => language.StatementNames.IsLessThan));
			DefineStatement(new StatementDefinition(typeof(Statements.IsLessThanOrEqualToStatement), language => language.StatementNames.IsLessThanOrEqualTo));
			DefineStatement(new StatementDefinition(typeof(Statements.IsGreaterThanStatement), language => language.StatementNames.IsGreaterThan));
			DefineStatement(new StatementDefinition(typeof(Statements.IsGreaterThanOrEqualToStatement), language => language.StatementNames.IsGreaterThanOrEqualTo));
			DefineStatement(new StatementDefinition(typeof(Statements.IsEqualToStatement), language => language.StatementNames.IsEqualTo));
			DefineStatement(new StatementDefinition(typeof(Statements.CausesStatement), language => language.StatementNames.Causes));
			DefineStatement(new StatementDefinition(typeof(Statements.MeanwhileStatement), language => language.StatementNames.Meanwhile));
			DefineStatement(new StatementDefinition(typeof(Statements.AfterStatement), language => language.StatementNames.After));
			DefineStatement(new StatementDefinition(typeof(Statements.BeforeStatement), language => language.StatementNames.Before));
		}

		public void DefineStatement(StatementDefinition statementDefinition)
		{
			StatementDefinitions[statementDefinition.StatementType] = statementDefinition;
		}
	}
}
