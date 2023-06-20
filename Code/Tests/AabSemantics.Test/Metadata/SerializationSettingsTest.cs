using System;
using System.Linq;
using NUnit.Framework;

using AabSemantics.Metadata;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Classification;

namespace AabSemantics.Test.Metadata
{
	[TestFixture]
	public class SerializationSettingsTest
	{
		[Test]
		public void SuccessfullyCreateXmlForAnswer()
		{
			// act & assert
			Assert.DoesNotThrow(() => new AnswerXmlSerializationSettings((arg, lang) => null, typeof(AabSemantics.Serialization.Xml.Answer)));
		}

		[Test]
		public void ImpossibleToCreateXmlForAnswerWithoutSerializer()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AnswerXmlSerializationSettings(null, typeof(AabSemantics.Serialization.Xml.Answer)));
		}

		[Test]
		public void ImpossibleToCreateXmlForAnswerWithoutType()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AnswerXmlSerializationSettings((arg, lang) => null, null));
		}

		[Test]
		public void ImpossibleToCreateXmlForAnswerWithWrongType()
		{
			// act & assert
			Assert.Throws<ArgumentException>(() => new AnswerXmlSerializationSettings((arg, lang) => null, typeof(object)));
			Assert.Throws<ArgumentException>(() => new AnswerXmlSerializationSettings((arg, lang) => null, typeof(AbstractXmlAnswer)));
		}

		[Test]
		public void SuccessfullyCreateJsonForAnswer()
		{
			// act & assert
			Assert.DoesNotThrow(() => new AnswerJsonSerializationSettings((arg, lang) => null, typeof(AabSemantics.Serialization.Json.Answer)));
		}

		[Test]
		public void ImpossibleToCreateJsonForAnswerWithoutSerializer()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AnswerJsonSerializationSettings(null, typeof(AabSemantics.Serialization.Json.Answer)));
		}

		[Test]
		public void ImpossibleToCreateJsonForAnswerWithoutType()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AnswerJsonSerializationSettings((arg, lang) => null, null));
		}

		[Test]
		public void ImpossibleToCreateJsonForAnswerWithWrongType()
		{
			// act & assert
			Assert.Throws<ArgumentException>(() => new AnswerJsonSerializationSettings((arg, lang) => null, typeof(object)));
			Assert.Throws<ArgumentException>(() => new AnswerJsonSerializationSettings((arg, lang) => null, typeof(AbstractJsonAnswer)));
		}

		[Test]
		public void SuccessfullyCreateXmlForAttribute()
		{
			// act & assert
			Assert.DoesNotThrow(() => new AttributeXmlSerializationSettings(new Modules.Boolean.Xml.IsValueAttribute()));
		}

		[Test]
		public void ImpossibleToCreateXmlForAttributeWithoutInstance()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AttributeXmlSerializationSettings(null));
		}

		[Test]
		public void SuccessfullyCreateJsonForAttribute()
		{
			// act & assert
			Assert.DoesNotThrow(() => new AttributeJsonSerializationSettings(new Modules.Boolean.Xml.IsValueAttribute()));
		}

		[Test]
		public void ImpossibleToCreateJsonForAttributeWithoutInstance()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AttributeJsonSerializationSettings(null));
		}

		[Test]
		public void SuccessfullyCreateXmlForStatement()
		{
			// act & assert
			Assert.DoesNotThrow(() => new StatementXmlSerializationSettings(arg => null, typeof(Modules.Classification.Xml.IsStatement)));
		}

		[Test]
		public void ImpossibleToCreateXmlForStatementWithoutSerializer()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new StatementXmlSerializationSettings(null, typeof(Modules.Classification.Xml.IsStatement)));
		}

		[Test]
		public void ImpossibleToCreateXmlForStatementWithoutType()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new StatementXmlSerializationSettings(arg => null, null));
		}

		[Test]
		public void ImpossibleToCreateXmlForStatementWithWrongType()
		{
			// act & assert
			Assert.Throws<ArgumentException>(() => new StatementXmlSerializationSettings(arg => null, typeof(object)));
			Assert.Throws<ArgumentException>(() => new StatementXmlSerializationSettings(arg => null, typeof(AbstractXmlStatement)));
		}

		[Test]
		public void SuccessfullyCreateJsonForStatement()
		{
			// act & assert
			Assert.DoesNotThrow(() => new StatementJsonSerializationSettings(arg => null, typeof(Modules.Classification.Json.IsStatement)));
		}

		[Test]
		public void ImpossibleToCreateJsonForStatementWithoutSerializer()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new StatementJsonSerializationSettings(null, typeof(Modules.Classification.Json.IsStatement)));
		}

		[Test]
		public void ImpossibleToCreateJsonForStatementWithoutType()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new StatementJsonSerializationSettings(arg => null, null));
		}

		[Test]
		public void ImpossibleToCreateJsonForStatementWithWrongType()
		{
			// act & assert
			Assert.Throws<ArgumentException>(() => new StatementJsonSerializationSettings(arg => null, typeof(object)));
			Assert.Throws<ArgumentException>(() => new StatementJsonSerializationSettings(arg => null, typeof(AbstractJsonStatement)));
		}

		[Test]
		public void SuccessfullyCreateXmlForQuestion()
		{
			// act & assert
			Assert.DoesNotThrow(() => new QuestionXmlSerializationSettings(arg => null, typeof(Modules.Boolean.Xml.CheckStatementQuestion)));
		}

		[Test]
		public void ImpossibleToCreateXmlForQuestionWithoutSerializer()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new QuestionXmlSerializationSettings(null, typeof(Modules.Boolean.Xml.CheckStatementQuestion)));
		}

		[Test]
		public void ImpossibleToCreateXmlForQuestionWithoutType()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new QuestionXmlSerializationSettings(arg => null, null));
		}

		[Test]
		public void ImpossibleToCreateXmlForQuestionWithWrongType()
		{
			// act & assert
			Assert.Throws<ArgumentException>(() => new QuestionXmlSerializationSettings(arg => null, typeof(object)));
			Assert.Throws<ArgumentException>(() => new QuestionXmlSerializationSettings(arg => null, typeof(AbstractXmlQuestion)));
		}

		[Test]
		public void SuccessfullyCreateJsonForQuestion()
		{
			// act & assert
			Assert.DoesNotThrow(() => new QuestionJsonSerializationSettings(arg => null, typeof(Modules.Boolean.Json.CheckStatementQuestion)));
		}

		[Test]
		public void ImpossibleToCreateJsonForQuestionWithoutSerializer()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new QuestionJsonSerializationSettings(null, typeof(Modules.Boolean.Json.CheckStatementQuestion)));
		}

		[Test]
		public void ImpossibleToCreateJsonForQuestionWithoutType()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new QuestionJsonSerializationSettings(arg => null, null));
		}

		[Test]
		public void ImpossibleToCreateJsonForQuestionWithWrongType()
		{
			// act & assert
			Assert.Throws<ArgumentException>(() => new QuestionJsonSerializationSettings(arg => null, typeof(object)));
			Assert.Throws<ArgumentException>(() => new QuestionJsonSerializationSettings(arg => null, typeof(AbstractJsonQuestion)));
		}

		[Test]
		public void CheckTypes()
		{
			// arrange
			var answerXmlType = typeof(AabSemantics.Serialization.Xml.Answer);
			var answerJsonType = typeof(AabSemantics.Serialization.Json.Answer);
			var attributeValue = new Modules.Boolean.Xml.IsValueAttribute();
			Type attributeXmlType = attributeValue.GetType();
			var statementXmlType = typeof(Modules.Classification.Xml.IsStatement);
			var statementJsonType = typeof(Modules.Classification.Json.IsStatement);
			var questionXmlType = typeof(Modules.Boolean.Xml.CheckStatementQuestion);
			var questionJsonType = typeof(Modules.Boolean.Json.CheckStatementQuestion);

			// act
			var answerXmlSettings = new AnswerXmlSerializationSettings((arg, lang) => null, answerXmlType);
			var answerJsonSettings = new AnswerJsonSerializationSettings((arg, lang) => null, answerJsonType);
			var attributeXmlSettings = new AttributeXmlSerializationSettings(attributeValue);
			var attributeJsonSettings = new AttributeJsonSerializationSettings(attributeValue);
			var statementXmlSettings = new StatementXmlSerializationSettings(arg => null, statementXmlType);
			var statementJsonSettings = new StatementJsonSerializationSettings(arg => null, statementJsonType);
			var questionXmlSettings = new QuestionXmlSerializationSettings(arg => null, questionXmlType);
			var questionJsonSettings = new QuestionJsonSerializationSettings(arg => null, questionJsonType);

			// assert
			Assert.AreSame(answerXmlType, answerXmlSettings.XmlType);
			Assert.AreSame(answerJsonType, answerJsonSettings.JsonType);
			Assert.AreSame(attributeXmlType, attributeXmlSettings.XmlType);
			Assert.IsNull(attributeJsonSettings.JsonType);
			Assert.AreSame(statementXmlType, statementXmlSettings.XmlType);
			Assert.AreSame(statementJsonType, statementJsonSettings.JsonType);
			Assert.AreSame(questionXmlType, questionXmlSettings.XmlType);
			Assert.AreSame(questionJsonType, questionJsonSettings.JsonType);
		}

		[Test]
		public void CheckUntypedExtensions()
		{
			// arrange
			new BooleanModule().RegisterMetadata();
			new ClassificationModule().RegisterMetadata();

			var answerDefinition = Repositories.Answers.Definitions.Values.First();
			var attributeDefinition = Repositories.Attributes.Definitions.Values.First();
			var statementDefinition = Repositories.Statements.Definitions.Values.First();
			var questionDefinition = Repositories.Questions.Definitions.Values.First();

			// act
			var answerXmlSerializationSettings = answerDefinition.GetXmlSerializationSettings();
			var attributeXmlSerializationSettings = attributeDefinition.GetXmlSerializationSettings();
			var statementXmlSerializationSettings = statementDefinition.GetXmlSerializationSettings();
			var questionXmlSerializationSettings = questionDefinition.GetXmlSerializationSettings();

			var answerJsonSerializationSettings = answerDefinition.GetJsonSerializationSettings();
			var attributeJsonSerializationSettings = attributeDefinition.GetJsonSerializationSettings();
			var statementJsonSerializationSettings = statementDefinition.GetJsonSerializationSettings();
			var questionJsonSerializationSettings = questionDefinition.GetJsonSerializationSettings();

			// assert
			Assert.AreSame(answerDefinition.GetXmlSerializationSettings<AnswerXmlSerializationSettings>(), answerXmlSerializationSettings);
			Assert.AreSame(attributeDefinition.GetXmlSerializationSettings<AttributeXmlSerializationSettings>(), attributeXmlSerializationSettings);
			Assert.AreSame(statementDefinition.GetXmlSerializationSettings<StatementXmlSerializationSettings>(), statementXmlSerializationSettings);
			Assert.AreSame(questionDefinition.GetXmlSerializationSettings<QuestionXmlSerializationSettings>(), questionXmlSerializationSettings);

			Assert.AreSame(answerDefinition.GetJsonSerializationSettings<AnswerJsonSerializationSettings>(), answerJsonSerializationSettings);
			Assert.AreSame(attributeDefinition.GetJsonSerializationSettings<AttributeJsonSerializationSettings>(), attributeJsonSerializationSettings);
			Assert.AreSame(statementDefinition.GetJsonSerializationSettings<StatementJsonSerializationSettings>(), statementJsonSerializationSettings);
			Assert.AreSame(questionDefinition.GetJsonSerializationSettings<QuestionJsonSerializationSettings>(), questionJsonSerializationSettings);
		}

		#region Abstract classes

		private abstract class AbstractXmlAnswer : AabSemantics.Serialization.Xml.Answer
		{ }

		private abstract class AbstractJsonAnswer : AabSemantics.Serialization.Json.Answer
		{ }

		private abstract class AbstractXmlStatement : AabSemantics.Serialization.Xml.Statement
		{ }

		private abstract class AbstractJsonStatement : AabSemantics.Serialization.Json.Statement
		{ }

		private abstract class AbstractXmlQuestion : AabSemantics.Serialization.Xml.Question
		{ }

		private abstract class AbstractJsonQuestion : AabSemantics.Serialization.Json.Question
		{ }

		#endregion
	}
}
