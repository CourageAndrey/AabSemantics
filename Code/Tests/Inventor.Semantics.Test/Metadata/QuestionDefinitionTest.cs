using System;

using NUnit.Framework;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Metadata;
using Inventor.Semantics.Modules.Boolean.Questions;
using Inventor.Semantics.Questions;

namespace Inventor.Semantics.Test.Metadata
{
	[TestFixture]
	public class QuestionDefinitionTest
	{
		[Test]
		public void ImpossibleToCreateDefinitionWithoutType()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new QuestionDefinition(
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
			Assert.Throws<ArgumentException>(() => new QuestionDefinition(
				typeof(string),
				language => language.Culture,
				question => new Modules.Boolean.Xml.CheckStatementQuestion((CheckStatementQuestion) question),
				question => new Modules.Boolean.Json.CheckStatementQuestion((CheckStatementQuestion) question),
				typeof(Modules.Boolean.Xml.CheckStatementQuestion),
				typeof(Modules.Boolean.Json.CheckStatementQuestion)));
			Assert.Throws<ArgumentException>(() => new QuestionDefinition(
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
			Assert.Throws<ArgumentNullException>(() => new QuestionDefinition(
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
			Assert.Throws<ArgumentNullException>(() => new QuestionDefinition(
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
			Assert.Throws<ArgumentNullException>(() => new QuestionDefinition(
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
			Assert.Throws<ArgumentNullException>(() => new QuestionDefinition(
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
			Assert.Throws<ArgumentNullException>(() => new QuestionDefinition(
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
			var definition = new QuestionDefinition(
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
	}
}
