using System;

using NUnit.Framework;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Metadata;
using Inventor.Semantics.Modules.Classification.Statements;
using Inventor.Semantics.Statements;

namespace Inventor.Semantics.Test.Metadata
{
	[TestFixture]
	public class StatementDefinitionTest
	{
		[Test]
		public void ImpossibleToCreateDefinitionWithoutType()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				null,
				language => language.Culture,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				StatementDefinition.NoConsistencyCheck));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithInvalidType()
		{
			// act & assert
			Assert.Throws<ArgumentException>(() => new TestStatementDefinition(
				typeof(Semantics.Questions.Question),
				language => language.Culture,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				StatementDefinition.NoConsistencyCheck));
			Assert.Throws<ArgumentException>(() => new StatementDefinition<Statement>(
				language => language.Culture,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				StatementDefinition<Statement>.NoConsistencyCheck));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithoutNameGetter()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new StatementDefinition<IsStatement>(
				language => language.Culture,
				null,
				typeof(Modules.Classification.Xml.IsStatement),
				StatementDefinition<IsStatement>.NoConsistencyCheck));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				StatementDefinition.NoConsistencyCheck));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithoutXmlType()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new StatementDefinition<IsStatement>(
				language => language.Culture,
				statement => null,
				null,
				StatementDefinition<IsStatement>.NoConsistencyCheck));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				null,
				typeof(Modules.Classification.Xml.IsStatement),
				StatementDefinition.NoConsistencyCheck));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				statement => null,
				null,
				StatementDefinition.NoConsistencyCheck));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithoutConsistencyChecker()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new StatementDefinition<IsStatement>(
				language => language.Culture,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				null));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				null));

			var definition = new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				StatementDefinition.NoConsistencyCheck);
			definition.CheckConsistency(null, null);
		}

		[Test]
		public void CheckName()
		{
			// arrange
			var definition = new StatementDefinition<IsStatement>(
				language => language.Culture,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				StatementDefinition<IsStatement>.NoConsistencyCheck);

			// act
			string name = definition.GetName(Language.Default);

			// assert
			Assert.AreEqual(Language.Default.Culture, name);
		}

		private class TestStatementDefinition : StatementDefinition
		{
			public TestStatementDefinition(
				Type type,
				Func<ILanguage, string> statementNameGetter,
				Func<IStatement, Semantics.Xml.Statement> statementXmlGetter,
				Type xmlType,
				StatementConsistencyCheckerDelegate consistencyChecker)
				: base(type, statementNameGetter, statementXmlGetter, xmlType, consistencyChecker)
			{ }
		}
	}
}
