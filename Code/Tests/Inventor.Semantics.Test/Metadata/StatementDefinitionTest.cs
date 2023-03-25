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
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
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
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition.NoConsistencyCheck));
			Assert.Throws<ArgumentException>(() => createStatementDefinition<Statement>(
				language => language.Culture,
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<Statement>.NoConsistencyCheck));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithoutNameGetter()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => createStatementDefinition<IsStatement>(
				null,
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement>.NoConsistencyCheck));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				null,
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition.NoConsistencyCheck));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithoutXmlType()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => createStatementDefinition<IsStatement>(
				language => language.Culture,
				statement => null,
				statement => null,
				null,
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement>.NoConsistencyCheck));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				statement => null,
				statement => null,
				null,
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition.NoConsistencyCheck));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithoutJsonType()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => createStatementDefinition<IsStatement>(
				language => language.Culture,
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				null,
				StatementDefinition<IsStatement>.NoConsistencyCheck));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				null,
				StatementDefinition.NoConsistencyCheck));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithoutXmlGetter()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => createStatementDefinition<IsStatement>(
				language => language.Culture,
				null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement>.NoConsistencyCheck));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition.NoConsistencyCheck));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithoutJsonGetter()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => createStatementDefinition<IsStatement>(
				language => language.Culture,
				statement => null,
				null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement>.NoConsistencyCheck));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				statement => null,
				null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition.NoConsistencyCheck));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithoutConsistencyChecker()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => createStatementDefinition<IsStatement>(
				language => language.Culture,
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				null));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				null));

			var definition = new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition.NoConsistencyCheck);
			definition.CheckConsistency(null, null);
		}

		[Test]
		public void CheckName()
		{
			// arrange
			var definition = createStatementDefinition<IsStatement>(
				language => language.Culture,
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement>.NoConsistencyCheck);

			// act
			string name = definition.GetName(Language.Default);

			// assert
			Assert.AreEqual(Language.Default.Culture, name);
		}

		private static StatementDefinition<StatementT> createStatementDefinition<StatementT>(
			Func<ILanguage, String> statementNameGetter,
			Func<IStatement, Semantics.Serialization.Xml.Statement> statementXmlGetter,
			Func<IStatement, Semantics.Serialization.Json.Statement> statementJsonGetter,
			Type xmlType,
			Type jsonType,
			StatementConsistencyCheckerDelegate<StatementT> consistencyChecker)
			where StatementT : IStatement
		{
			var statementDefinition = new StatementDefinition<StatementT>(statementNameGetter, consistencyChecker);
			statementDefinition.SerializeToXml(statementXmlGetter, xmlType);
			statementDefinition.SerializeToJson(statementJsonGetter, jsonType);
			return statementDefinition;
		}

		private class TestStatementDefinition : StatementDefinition
		{
			public TestStatementDefinition(
				Type type,
				Func<ILanguage, string> statementNameGetter,
				Func<IStatement, Semantics.Serialization.Xml.Statement> statementXmlGetter,
				Func<IStatement, Semantics.Serialization.Json.Statement> statementJsonGetter,
				Type xmlType,
				Type jsonType,
				StatementConsistencyCheckerDelegate consistencyChecker)
				: base(type, statementNameGetter, consistencyChecker)
			{
				this.SerializeToXml(statementXmlGetter, xmlType);
				this.SerializeToJson(statementJsonGetter, jsonType);
			}
		}
	}
}
