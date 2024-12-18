using System;

using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Statements;

namespace AabSemantics.Tests.Metadata
{
	[TestFixture]
	public class StatementDefinitionTest
	{
		[Test]
		public void GivenNoType_WhenTryToCreate_ThenFail()
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
		public void GivenInvalidType_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentException>(() => new TestStatementDefinition(
				typeof(AabSemantics.Questions.Question),
				language => language.Culture,
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition.NoConsistencyCheck));
			Assert.Throws<ArgumentException>(() => CreateStatementDefinition<Statement>(
				language => language.Culture,
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<Statement>.NoConsistencyCheck));
		}

		[Test]
		public void GivenNoNameGetter_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => CreateStatementDefinition<IsStatement>(
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
		public void GivenNoXmlType_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => CreateStatementDefinition<IsStatement>(
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
		public void GivenNoJsonType_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => CreateStatementDefinition<IsStatement>(
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
		public void GivenNoXmlGetter_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => CreateStatementDefinition<IsStatement>(
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
		public void GivenNoJsonGetter_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => CreateStatementDefinition<IsStatement>(
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
		public void GivenNoConsistencyChecker_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => CreateStatementDefinition<IsStatement>(
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
		public void GivenCorrectDefinition_WhenGetName_ThenReturnIt()
		{
			// arrange
			var definition = CreateStatementDefinition<IsStatement>(
				language => language.Culture,
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement>.NoConsistencyCheck);

			// act
			string name = definition.GetName(Language.Default);

			// assert
			Assert.That(name, Is.EqualTo(Language.Default.Culture));
		}

		private static StatementDefinition<StatementT> CreateStatementDefinition<StatementT>(
			Func<ILanguage, String> nameGetter,
			Func<IStatement, AabSemantics.Serialization.Xml.Statement> xmlSerializer,
			Func<IStatement, AabSemantics.Serialization.Json.Statement> jsonSerializer,
			Type xmlType,
			Type jsonType,
			StatementConsistencyCheckerDelegate<StatementT> consistencyChecker)
			where StatementT : IStatement
		{
			var statementDefinition = new StatementDefinition<StatementT>(nameGetter, consistencyChecker);
			statementDefinition.SerializeToXml(xmlSerializer, xmlType);
			statementDefinition.SerializeToJson(jsonSerializer, jsonType);
			return statementDefinition;
		}

		private class TestStatementDefinition : StatementDefinition
		{
			public TestStatementDefinition(
				Type type,
				Func<ILanguage, string> nameGetter,
				Func<IStatement, AabSemantics.Serialization.Xml.Statement> xmlSerializer,
				Func<IStatement, AabSemantics.Serialization.Json.Statement> jsonSerializer,
				Type xmlType,
				Type jsonType,
				StatementConsistencyCheckerDelegate consistencyChecker)
				: base(type, nameGetter, consistencyChecker)
			{
				this.SerializeToXml(xmlSerializer, xmlType);
				this.SerializeToJson(jsonSerializer, jsonType);
			}
		}
	}
}
