using System;

using NUnit.Framework;

using Inventor.Semantics.Metadata;

namespace Inventor.Semantics.Test.Metadata
{
	[TestFixture]
	public class SerializationSettingsTest
	{
		[Test]
		public void SuccessfullyCreateXmlForAnswer()
		{
			// act & assert
			Assert.DoesNotThrow(() => new AnswerXmlSerializationSettings((arg, lang) => null, typeof(Semantics.Serialization.Xml.Answer)));
		}

		[Test]
		public void ImpossibleToCreateXmlForAnswerWithoutSerializer()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AnswerXmlSerializationSettings(null, typeof(Semantics.Serialization.Xml.Answer)));
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
			Assert.DoesNotThrow(() => new AnswerJsonSerializationSettings((arg, lang) => null, typeof(Semantics.Serialization.Json.Answer)));
		}

		[Test]
		public void ImpossibleToCreateJsonForAnswerWithoutSerializer()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new AnswerJsonSerializationSettings(null, typeof(Semantics.Serialization.Json.Answer)));
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

		#region Abstract classes

		private abstract class AbstractXmlAnswer : Semantics.Serialization.Xml.Answer
		{ }

		private abstract class AbstractJsonAnswer : Semantics.Serialization.Json.Answer
		{ }

		private abstract class AbstractXmlStatement : Semantics.Serialization.Xml.Statement
		{ }

		private abstract class AbstractJsonStatement : Semantics.Serialization.Json.Statement
		{ }

		private abstract class AbstractXmlQuestion : Semantics.Serialization.Xml.Question
		{ }

		private abstract class AbstractJsonQuestion : Semantics.Serialization.Json.Question
		{ }

		#endregion
	}
}
