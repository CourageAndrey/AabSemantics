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
		public void ImpossibleToCreateDefinitionWithoutType()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => createQuestionDefinition(
				null,
				language => language.Culture,
				question => new Modules.Boolean.Xml.CheckStatementQuestion((CheckStatementQuestion) question),
				question => new Modules.Boolean.Json.CheckStatementQuestion((CheckStatementQuestion) question),
				typeof(Modules.Boolean.Xml.CheckStatementQuestion),
				typeof(Modules.Boolean.Json.CheckStatementQuestion)));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithInvalidType()
		{
			// act & assert
			Assert.Throws<ArgumentException>(() => createQuestionDefinition(
				typeof(string),
				language => language.Culture,
				question => new Modules.Boolean.Xml.CheckStatementQuestion((CheckStatementQuestion) question),
				question => new Modules.Boolean.Json.CheckStatementQuestion((CheckStatementQuestion) question),
				typeof(Modules.Boolean.Xml.CheckStatementQuestion),
				typeof(Modules.Boolean.Json.CheckStatementQuestion)));
			Assert.Throws<ArgumentException>(() => createQuestionDefinition(
				typeof(Question),
				language => language.Culture,
				question => new Modules.Boolean.Xml.CheckStatementQuestion((CheckStatementQuestion) question),
				question => new Modules.Boolean.Json.CheckStatementQuestion((CheckStatementQuestion) question),
				typeof(Modules.Boolean.Xml.CheckStatementQuestion),
				typeof(Modules.Boolean.Json.CheckStatementQuestion)));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithoutNameGetter()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => createQuestionDefinition(
				typeof(CheckStatementQuestion),
				null,
				question => new Modules.Boolean.Xml.CheckStatementQuestion((CheckStatementQuestion) question),
				question => new Modules.Boolean.Json.CheckStatementQuestion((CheckStatementQuestion) question),
				typeof(Modules.Boolean.Xml.CheckStatementQuestion),
				typeof(Modules.Boolean.Json.CheckStatementQuestion)));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithoutXmlGetter()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => createQuestionDefinition(
				typeof(CheckStatementQuestion),
				language => language.Culture,
				null,
				question => new Modules.Boolean.Json.CheckStatementQuestion((CheckStatementQuestion) question),
				typeof(Modules.Boolean.Xml.CheckStatementQuestion),
				typeof(Modules.Boolean.Json.CheckStatementQuestion)));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithoutJsonGetter()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => createQuestionDefinition(
				typeof(CheckStatementQuestion),
				language => language.Culture,
				question => new Modules.Boolean.Xml.CheckStatementQuestion((CheckStatementQuestion) question),
				null,
				typeof(Modules.Boolean.Xml.CheckStatementQuestion),
				typeof(Modules.Boolean.Json.CheckStatementQuestion)));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithoutXmlType()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => createQuestionDefinition(
				typeof(CheckStatementQuestion),
				language => language.Culture,
				question => new Modules.Boolean.Xml.CheckStatementQuestion((CheckStatementQuestion) question),
				question => new Modules.Boolean.Json.CheckStatementQuestion((CheckStatementQuestion) question),
				null,
				typeof(Modules.Boolean.Json.CheckStatementQuestion)));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithoutJsonType()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => createQuestionDefinition(
				typeof(CheckStatementQuestion),
				language => language.Culture,
				question => new Modules.Boolean.Xml.CheckStatementQuestion((CheckStatementQuestion) question),
				question => new Modules.Boolean.Json.CheckStatementQuestion((CheckStatementQuestion) question),
				typeof(Modules.Boolean.Xml.CheckStatementQuestion),
				null));
		}

		[Test]
		public void CheckName()
		{
			// arrange
			var definition = createQuestionDefinition(
				typeof(CheckStatementQuestion),
				language => language.Culture,
				question => new Modules.Boolean.Xml.CheckStatementQuestion((CheckStatementQuestion) question),
				question => new Modules.Boolean.Json.CheckStatementQuestion((CheckStatementQuestion) question),
				typeof(Modules.Boolean.Xml.CheckStatementQuestion),
				typeof(Modules.Boolean.Json.CheckStatementQuestion));

			// act
			string name = definition.GetName(Language.Default);

			// assert
			Assert.AreEqual(Language.Default.Culture, name);
		}

		private static QuestionDefinition createQuestionDefinition(
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
