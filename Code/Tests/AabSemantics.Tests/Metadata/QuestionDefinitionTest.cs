using System;

using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Modules.Boolean.Questions;
using AabSemantics.Questions;

namespace AabSemantics.Tests.Metadata
{
	[TestFixture]
	public class QuestionDefinitionTest
	{
		[Test]
		public void GivenNoType_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => CreateQuestionDefinition(
				null,
				language => language.Culture,
				question => new Modules.Boolean.Xml.CheckStatementQuestion((CheckStatementQuestion) question),
				question => new Modules.Boolean.Json.CheckStatementQuestion((CheckStatementQuestion) question),
				typeof(Modules.Boolean.Xml.CheckStatementQuestion),
				typeof(Modules.Boolean.Json.CheckStatementQuestion)));
		}

		[Test]
		public void GivenInvalidType_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentException>(() => CreateQuestionDefinition(
				typeof(string),
				language => language.Culture,
				question => new Modules.Boolean.Xml.CheckStatementQuestion((CheckStatementQuestion) question),
				question => new Modules.Boolean.Json.CheckStatementQuestion((CheckStatementQuestion) question),
				typeof(Modules.Boolean.Xml.CheckStatementQuestion),
				typeof(Modules.Boolean.Json.CheckStatementQuestion)));
			Assert.Throws<ArgumentException>(() => CreateQuestionDefinition(
				typeof(Question),
				language => language.Culture,
				question => new Modules.Boolean.Xml.CheckStatementQuestion((CheckStatementQuestion) question),
				question => new Modules.Boolean.Json.CheckStatementQuestion((CheckStatementQuestion) question),
				typeof(Modules.Boolean.Xml.CheckStatementQuestion),
				typeof(Modules.Boolean.Json.CheckStatementQuestion)));
		}

		[Test]
		public void GivenNoNameGetter_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => CreateQuestionDefinition(
				typeof(CheckStatementQuestion),
				null,
				question => new Modules.Boolean.Xml.CheckStatementQuestion((CheckStatementQuestion) question),
				question => new Modules.Boolean.Json.CheckStatementQuestion((CheckStatementQuestion) question),
				typeof(Modules.Boolean.Xml.CheckStatementQuestion),
				typeof(Modules.Boolean.Json.CheckStatementQuestion)));
		}

		[Test]
		public void GivenNoXmlGetter_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => CreateQuestionDefinition(
				typeof(CheckStatementQuestion),
				language => language.Culture,
				null,
				question => new Modules.Boolean.Json.CheckStatementQuestion((CheckStatementQuestion) question),
				typeof(Modules.Boolean.Xml.CheckStatementQuestion),
				typeof(Modules.Boolean.Json.CheckStatementQuestion)));
		}

		[Test]
		public void GivenNoJsonGetter_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => CreateQuestionDefinition(
				typeof(CheckStatementQuestion),
				language => language.Culture,
				question => new Modules.Boolean.Xml.CheckStatementQuestion((CheckStatementQuestion) question),
				null,
				typeof(Modules.Boolean.Xml.CheckStatementQuestion),
				typeof(Modules.Boolean.Json.CheckStatementQuestion)));
		}

		[Test]
		public void GivenNoXmlType_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => CreateQuestionDefinition(
				typeof(CheckStatementQuestion),
				language => language.Culture,
				question => new Modules.Boolean.Xml.CheckStatementQuestion((CheckStatementQuestion) question),
				question => new Modules.Boolean.Json.CheckStatementQuestion((CheckStatementQuestion) question),
				null,
				typeof(Modules.Boolean.Json.CheckStatementQuestion)));
		}

		[Test]
		public void GivenNoJsonType_WhenTryToCreate_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => CreateQuestionDefinition(
				typeof(CheckStatementQuestion),
				language => language.Culture,
				question => new Modules.Boolean.Xml.CheckStatementQuestion((CheckStatementQuestion) question),
				question => new Modules.Boolean.Json.CheckStatementQuestion((CheckStatementQuestion) question),
				typeof(Modules.Boolean.Xml.CheckStatementQuestion),
				null));
		}

		[Test]
		public void GivenCorrectDefinition_WhenGetName_ThenReturnIt()
		{
			// arrange
			var definition = CreateQuestionDefinition(
				typeof(CheckStatementQuestion),
				language => language.Culture,
				question => new Modules.Boolean.Xml.CheckStatementQuestion((CheckStatementQuestion) question),
				question => new Modules.Boolean.Json.CheckStatementQuestion((CheckStatementQuestion) question),
				typeof(Modules.Boolean.Xml.CheckStatementQuestion),
				typeof(Modules.Boolean.Json.CheckStatementQuestion));

			// act
			string name = definition.GetName(Language.Default);

			// assert
			Assert.That(name, Is.EqualTo(Language.Default.Culture));
		}

		private static QuestionDefinition CreateQuestionDefinition(
			Type type,
			Func<ILanguage, String> nameGetter,
			Func<IQuestion, AabSemantics.Serialization.Xml.Question> xmlSerializer,
			Func<IQuestion, AabSemantics.Serialization.Json.Question> jsonSerializer,
			Type xmlType,
			Type jsonType)
		{
			return new QuestionDefinition(type, nameGetter)
				.SerializeToXml(xmlSerializer, xmlType)
				.SerializeToJson(jsonSerializer, jsonType);
		}
	}
}
