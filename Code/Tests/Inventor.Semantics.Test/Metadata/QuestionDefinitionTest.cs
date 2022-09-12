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
			Assert.Throws<ArgumentNullException>(() => new QuestionDefinition(null, language => language.Culture));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithInvalidType()
		{
			// act & assert
			Assert.Throws<ArgumentException>(() => new QuestionDefinition(typeof(string), language => language.Culture));
			Assert.Throws<ArgumentException>(() => new QuestionDefinition(typeof(Question), language => language.Culture));
		}

		[Test]
		public void ImpossibleToCreateDefinitionWithoutNameGetter()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new QuestionDefinition(typeof(CheckStatementQuestion), null));
		}

		[Test]
		public void CheckName()
		{
			// arrange
			var definition = new QuestionDefinition(typeof(CheckStatementQuestion), language => language.Culture);

			// act
			string name = definition.GetName(Language.Default);

			// assert
			Assert.AreEqual(Language.Default.Culture, name);
		}
	}
}
