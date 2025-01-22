using System;
using System.Collections.Generic;

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
				language => null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
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
				language => null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition.NoConsistencyCheck));
			Assert.Throws<ArgumentException>(() => CreateStatementDefinition<Statement>(
				language => language.Culture,
				language => null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
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
				language => null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement>.NoConsistencyCheck));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				null,
				language => null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition.NoConsistencyCheck));
		}

		[Test]
		public void GivenNoFormatTrue_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => CreateStatementDefinition<IsStatement>(
				language => language.Culture,
				null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement>.NoConsistencyCheck));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition.NoConsistencyCheck));
		}

		[Test]
		public void GivenNoFormatFalse_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => CreateStatementDefinition<IsStatement>(
				language => language.Culture,
				language => null,
				null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement>.NoConsistencyCheck));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				language => null,
				null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition.NoConsistencyCheck));
		}

		[Test]
		public void GivenNoFormatQuestion_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => CreateStatementDefinition<IsStatement>(
				language => language.Culture,
				language => null,
				language => null,
				null,
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement>.NoConsistencyCheck));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				language => null,
				language => null,
				null,
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition.NoConsistencyCheck));
		}

		[Test]
		public void GivenNoFormatParameters_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => CreateStatementDefinition<IsStatement>(
				language => language.Culture,
				language => null,
				language => null,
				language => null,
				null,
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement>.NoConsistencyCheck));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				language => null,
				language => null,
				language => null,
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
				language => null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				statement => null,
				null,
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement>.NoConsistencyCheck));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				language => null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
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
				language => null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				null,
				StatementDefinition<IsStatement>.NoConsistencyCheck));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				language => null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
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
				language => null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
				null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement>.NoConsistencyCheck));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				language => null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
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
				language => null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement>.NoConsistencyCheck));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				language => null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
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
				language => null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				null));
			Assert.Throws<ArgumentNullException>(() => new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				language => null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				null));

			var definition = new TestStatementDefinition(
				typeof(IsStatement),
				language => language.Culture,
				language => null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
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
				language => null,
				language => null,
				language => null,
				statement => new Dictionary<string, IKnowledge>(),
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
			Func<ILanguage, String> formatTrue,
			Func<ILanguage, String> formatFalse,
			Func<ILanguage, String> formatQuestion,
			Func<StatementT, IDictionary<String, IKnowledge>> getDescriptionParameters,
			Func<IStatement, AabSemantics.Serialization.Xml.Statement> xmlSerializer,
			Func<IStatement, AabSemantics.Serialization.Json.Statement> jsonSerializer,
			Type xmlType,
			Type jsonType,
			StatementConsistencyCheckerDelegate<StatementT> consistencyChecker)
			where StatementT : class, IStatement
		{
			var statementDefinition = new StatementDefinition<StatementT>(
				nameGetter,
				formatTrue,
				formatFalse,
				formatQuestion,
				getDescriptionParameters,
				consistencyChecker);
			statementDefinition.SerializeToXml<StatementT>(xmlSerializer, xmlType);
			statementDefinition.SerializeToJson<StatementT>(jsonSerializer, jsonType);
			return statementDefinition;
		}

		private class TestStatementDefinition : StatementDefinition
		{
			public TestStatementDefinition(
				Type type,
				Func<ILanguage, string> nameGetter,
				Func<ILanguage, String> formatTrue,
				Func<ILanguage, String> formatFalse,
				Func<ILanguage, String> formatQuestion,
				Func<IStatement, IDictionary<String, IKnowledge>> getDescriptionParameters,
				Func<IStatement, AabSemantics.Serialization.Xml.Statement> xmlSerializer,
				Func<IStatement, AabSemantics.Serialization.Json.Statement> jsonSerializer,
				Type xmlType,
				Type jsonType,
				StatementConsistencyCheckerDelegate consistencyChecker)
				: base(
					type,
					nameGetter,
					formatTrue,
					formatFalse,
					formatQuestion,
					getDescriptionParameters,
					consistencyChecker)
			{
				this.SerializeToXml(xmlSerializer, xmlType);
				this.SerializeToJson(jsonSerializer, jsonType);
			}
		}
	}
}
