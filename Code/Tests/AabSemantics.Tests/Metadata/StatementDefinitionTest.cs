using System;
using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Statements;
using AabSemantics.Modules.Classification;

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
			Assert.Throws<ArgumentException>(() => CreateStatementDefinition<Statement, Modules.Classification.Localization.ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements, Modules.Classification.Localization.ILanguageStatementsPart>(
				language => language.ToString(),
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<Statement, Modules.Classification.Localization.ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements, Modules.Classification.Localization.ILanguageStatementsPart>.NoConsistencyCheck));
		}

		[Test]
		public void GivenNoPartGetter_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => CreateStatementDefinition<IsStatement, Modules.Classification.Localization.ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements, Modules.Classification.Localization.ILanguageStatementsPart>(
				null,
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement, Modules.Classification.Localization.ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements, Modules.Classification.Localization.ILanguageStatementsPart>.NoConsistencyCheck));
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
			Assert.Throws<ArgumentNullException>(() => CreateStatementDefinition<IsStatement, Modules.Classification.Localization.ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements, Modules.Classification.Localization.ILanguageStatementsPart>(
				language => language.ToString(),
				null,
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement, Modules.Classification.Localization.ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements, Modules.Classification.Localization.ILanguageStatementsPart>.NoConsistencyCheck));
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
			Assert.Throws<ArgumentNullException>(() => CreateStatementDefinition<IsStatement, Modules.Classification.Localization.ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements, Modules.Classification.Localization.ILanguageStatementsPart>(
				language => language.ToString(),
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				statement => null,
				null,
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement, Modules.Classification.Localization.ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements, Modules.Classification.Localization.ILanguageStatementsPart>.NoConsistencyCheck));
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
			Assert.Throws<ArgumentNullException>(() => CreateStatementDefinition<IsStatement, Modules.Classification.Localization.ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements, Modules.Classification.Localization.ILanguageStatementsPart>(
				language => language.ToString(),
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				null,
				StatementDefinition<IsStatement, Modules.Classification.Localization.ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements, Modules.Classification.Localization.ILanguageStatementsPart>.NoConsistencyCheck));
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
			Assert.Throws<ArgumentNullException>(() => CreateStatementDefinition<IsStatement, Modules.Classification.Localization.ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements, Modules.Classification.Localization.ILanguageStatementsPart>(
				language => language.ToString(),
				statement => new Dictionary<string, IKnowledge>(),
				null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement, Modules.Classification.Localization.ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements, Modules.Classification.Localization.ILanguageStatementsPart>.NoConsistencyCheck));
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
			Assert.Throws<ArgumentNullException>(() => CreateStatementDefinition<IsStatement, Modules.Classification.Localization.ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements, Modules.Classification.Localization.ILanguageStatementsPart>(
				language => language.ToString(),
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement, Modules.Classification.Localization.ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements, Modules.Classification.Localization.ILanguageStatementsPart>.NoConsistencyCheck));
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
			Assert.Throws<ArgumentNullException>(() => CreateStatementDefinition<IsStatement, Modules.Classification.Localization.ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements, Modules.Classification.Localization.ILanguageStatementsPart>(
				language => language.ToString(),
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
			new ClassificationModule().RegisterMetadata();

			var definition = CreateStatementDefinition<IsStatement, Modules.Classification.Localization.ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements, Modules.Classification.Localization.ILanguageStatementsPart>(
				language => language.ToString(),
				statement => new Dictionary<string, IKnowledge>(),
				statement => null,
				statement => null,
				typeof(Modules.Classification.Xml.IsStatement),
				typeof(Modules.Classification.Json.IsStatement),
				StatementDefinition<IsStatement, Modules.Classification.Localization.ILanguageClassificationModule, Modules.Classification.Localization.ILanguageStatements, Modules.Classification.Localization.ILanguageStatementsPart>.NoConsistencyCheck);

			// act
			string name = definition.GetName(Language.Default);

			// assert
			Assert.That(name, Is.EqualTo(typeof(Modules.Classification.Localization.LanguageStatementsPart).ToString()));
		}

		private static StatementDefinition<StatementT, ModuleT, LanguageStatementsT, PartT> CreateStatementDefinition<StatementT, ModuleT, LanguageStatementsT, PartT>(
			Func<PartT, String> partGetter,
			Func<StatementT, IDictionary<String, IKnowledge>> getDescriptionParameters,
			Func<IStatement, AabSemantics.Serialization.Xml.Statement> xmlSerializer,
			Func<IStatement, AabSemantics.Serialization.Json.Statement> jsonSerializer,
			Type xmlType,
			Type jsonType,
			StatementConsistencyCheckerDelegate<StatementT> consistencyChecker)
			where StatementT : class, IStatement
			where ModuleT : ILanguageStatementsExtension<LanguageStatementsT>
			where LanguageStatementsT : ILanguageExtensionStatements<PartT>
		{
			var statementDefinition = new StatementDefinition<StatementT, ModuleT, LanguageStatementsT, PartT>(
				partGetter,
				getDescriptionParameters,
				consistencyChecker);
			statementDefinition.SerializeToXml<StatementT, ModuleT, LanguageStatementsT, PartT>(xmlSerializer, xmlType);
			statementDefinition.SerializeToJson<StatementT, ModuleT, LanguageStatementsT, PartT>(jsonSerializer, jsonType);
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
