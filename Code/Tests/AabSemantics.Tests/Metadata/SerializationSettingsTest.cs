﻿using System;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Metadata;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Classification;

namespace AabSemantics.Tests.Metadata
{
	[TestFixture]
	public class SerializationSettingsTest
	{
		[Test]
		public void GivenAllSet_WhenCreateXmlForAnswer_ThenSucceed()
		{
			// act & assert
			Assert.DoesNotThrow(() => new AnswerXmlSerializationSettings((arg, lang) => null, typeof(AabSemantics.Serialization.Xml.Answer)));
		}

		[Test]
		public void GivenNoSerializer_WhenTryToCreateXmlForAnswer_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AnswerXmlSerializationSettings(null, typeof(AabSemantics.Serialization.Xml.Answer)));
		}

		[Test]
		public void GivenNoType_WhenTryToCreateXmlForAnswer_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AnswerXmlSerializationSettings((arg, lang) => null, null));
		}

		[Test]
		public void GivenWrongType_WhenTryToCreateXmlForAnswer_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentException>(() => new AnswerXmlSerializationSettings((arg, lang) => null, typeof(object)));
			Assert.Throws<ArgumentException>(() => new AnswerXmlSerializationSettings((arg, lang) => null, typeof(AbstractXmlAnswer)));
		}

		[Test]
		public void GivenAllSet_WhenCreateJsonForAnswer_ThenSucceed()
		{
			// act & assert
			Assert.DoesNotThrow(() => new AnswerJsonSerializationSettings((arg, lang) => null, typeof(AabSemantics.Serialization.Json.Answer)));
		}

		[Test]
		public void GivenNoSerializer_WhenTryToCreateJsonForAnswer_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AnswerJsonSerializationSettings(null, typeof(AabSemantics.Serialization.Json.Answer)));
		}

		[Test]
		public void GivenNoType_WhenTryToCreateJsonForAnswer_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AnswerJsonSerializationSettings((arg, lang) => null, null));
		}

		[Test]
		public void GivenWrongType_WhenTryToCreateJsonForAnswer_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentException>(() => new AnswerJsonSerializationSettings((arg, lang) => null, typeof(object)));
			Assert.Throws<ArgumentException>(() => new AnswerJsonSerializationSettings((arg, lang) => null, typeof(AbstractJsonAnswer)));
		}

		[Test]
		public void GivenAllSet_WhenCreateXmlForAttribute_ThenSucceed()
		{
			// act & assert
			Assert.DoesNotThrow(() => new AttributeXmlSerializationSettings(new Modules.Boolean.Xml.IsValueAttribute()));
		}

		[Test]
		public void GivenNoInstance_WhenTryToCreateXmlForAttribute_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AttributeXmlSerializationSettings(null));
		}

		[Test]
		public void GivenAllSet_WhenCreateJsonForAttribute_ThenSucceed()
		{
			// act & assert
			Assert.DoesNotThrow(() => new AttributeJsonSerializationSettings(new Modules.Boolean.Xml.IsValueAttribute()));
		}

		[Test]
		public void GivenNoInstance_WhenTryToCreateJsonForAttribute_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AttributeJsonSerializationSettings(null));
		}

		[Test]
		public void GivenAllSet_WhenCreateXmlForStatement_ThenSucceed()
		{
			// act & assert
			Assert.DoesNotThrow(() => new StatementXmlSerializationSettings(arg => null, typeof(Modules.Classification.Xml.IsStatement)));
		}

		[Test]
		public void GivenNoSerializer_WhenTryToCreateXmlForStatement_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new StatementXmlSerializationSettings(null, typeof(Modules.Classification.Xml.IsStatement)));
		}

		[Test]
		public void GivenNoType_WhenTryToCreateXmlForStatement_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new StatementXmlSerializationSettings(arg => null, null));
		}

		[Test]
		public void GivenWrongType_WhenTryToCreateXmlForStatement_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentException>(() => new StatementXmlSerializationSettings(arg => null, typeof(object)));
			Assert.Throws<ArgumentException>(() => new StatementXmlSerializationSettings(arg => null, typeof(AbstractXmlStatement)));
		}

		[Test]
		public void GivenAllSet_WhenCreateJsonForStatement_ThenSucceed()
		{
			// act & assert
			Assert.DoesNotThrow(() => new StatementJsonSerializationSettings(arg => null, typeof(Modules.Classification.Json.IsStatement)));
		}

		[Test]
		public void GivenNoSerializer_WhenTryToCreateJsonForStatement_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new StatementJsonSerializationSettings(null, typeof(Modules.Classification.Json.IsStatement)));
		}

		[Test]
		public void GivenNoType_WhenTryToCreateJsonForStatement_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new StatementJsonSerializationSettings(arg => null, null));
		}

		[Test]
		public void GivenWrongType_WhenTryToCreateJsonForStatement_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentException>(() => new StatementJsonSerializationSettings(arg => null, typeof(object)));
			Assert.Throws<ArgumentException>(() => new StatementJsonSerializationSettings(arg => null, typeof(AbstractJsonStatement)));
		}

		[Test]
		public void GivenAllSet_WhenCreateXmlForQuestion_ThenSucceed()
		{
			// act & assert
			Assert.DoesNotThrow(() => new QuestionXmlSerializationSettings(arg => null, typeof(Modules.Boolean.Xml.CheckStatementQuestion)));
		}

		[Test]
		public void GivenNoSerializer_WhenTryToCreateXmlForQuestion_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new QuestionXmlSerializationSettings(null, typeof(Modules.Boolean.Xml.CheckStatementQuestion)));
		}

		[Test]
		public void GivenNoType_WhenTryToCreateXmlForQuestion_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new QuestionXmlSerializationSettings(arg => null, null));
		}

		[Test]
		public void GivenWrongType_WhenTryToCreateXmlForQuestion_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentException>(() => new QuestionXmlSerializationSettings(arg => null, typeof(object)));
			Assert.Throws<ArgumentException>(() => new QuestionXmlSerializationSettings(arg => null, typeof(AbstractXmlQuestion)));
		}

		[Test]
		public void GivenAllSet_WhenCreateJsonForQuestion_ThenSucceed()
		{
			// act & assert
			Assert.DoesNotThrow(() => new QuestionJsonSerializationSettings(arg => null, typeof(Modules.Boolean.Json.CheckStatementQuestion)));
		}

		[Test]
		public void GivenNoSerializer_WhenTryToCreateJsonForQuestion_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new QuestionJsonSerializationSettings(null, typeof(Modules.Boolean.Json.CheckStatementQuestion)));
		}

		[Test]
		public void GivenNoType_WhenTryToCreateJsonForQuestion_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new QuestionJsonSerializationSettings(arg => null, null));
		}

		[Test]
		public void GivenWrongType_WhenTryToCreateJsonForQuestion_ThenFail()
		{
			// act & assert
			Assert.Throws<ArgumentException>(() => new QuestionJsonSerializationSettings(arg => null, typeof(object)));
			Assert.Throws<ArgumentException>(() => new QuestionJsonSerializationSettings(arg => null, typeof(AbstractJsonQuestion)));
		}

		[Test]
		public void GivenXmlAndJsonTypes_WhenCreateSerializationSettings_ThenStoreTheseTypes()
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
			Assert.That(answerXmlType, Is.SameAs(answerXmlSettings.XmlType));
			Assert.That(answerJsonType, Is.SameAs(answerJsonSettings.JsonType));
			Assert.That(attributeXmlType, Is.SameAs(attributeXmlSettings.XmlType));
			Assert.That(attributeJsonSettings.JsonType, Is.Null);
			Assert.That(statementXmlType, Is.SameAs(statementXmlSettings.XmlType));
			Assert.That(statementJsonType, Is.SameAs(statementJsonSettings.JsonType));
			Assert.That(questionXmlType, Is.SameAs(questionXmlSettings.XmlType));
			Assert.That(questionJsonType, Is.SameAs(questionJsonSettings.JsonType));
		}

		[Test]
		public void GivenTypedAndUntypedExtensions_WhenCallThem_ThenWorkTheSame()
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
			Assert.That(answerDefinition.GetSerializationSettings<AnswerXmlSerializationSettings>(), Is.SameAs(answerXmlSerializationSettings));
			Assert.That(attributeDefinition.GetSerializationSettings<AttributeXmlSerializationSettings>(), Is.SameAs(attributeXmlSerializationSettings));
			Assert.That(statementDefinition.GetSerializationSettings<StatementXmlSerializationSettings>(), Is.SameAs(statementXmlSerializationSettings));
			Assert.That(questionDefinition.GetSerializationSettings<QuestionXmlSerializationSettings>(), Is.SameAs(questionXmlSerializationSettings));

			Assert.That(answerDefinition.GetSerializationSettings<AnswerJsonSerializationSettings>(), Is.SameAs(answerJsonSerializationSettings));
			Assert.That(attributeDefinition.GetSerializationSettings<AttributeJsonSerializationSettings>(), Is.SameAs(attributeJsonSerializationSettings));
			Assert.That(statementDefinition.GetSerializationSettings<StatementJsonSerializationSettings>(), Is.SameAs(statementJsonSerializationSettings));
			Assert.That(questionDefinition.GetSerializationSettings<QuestionJsonSerializationSettings>(), Is.SameAs(questionJsonSerializationSettings));
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
