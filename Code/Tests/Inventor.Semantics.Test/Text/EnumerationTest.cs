using NUnit.Framework;

using Inventor.Semantics.Concepts;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Text.Containers;

namespace Inventor.Semantics.Test.Text
{
	[TestFixture]
	public class EnumerationTest
	{
		[Test]
		public void Enumerate()
		{
			// arrange
			var knowledge = new IKnowledge[]
			{
				"Abc".CreateConcept(),
				"Def".CreateConcept(),
				"Uvw".CreateConcept(),
				"Xyz".CreateConcept(),
			};

			var language = Language.Default;
			var representer = TextRepresenters.PlainString;

			// act
			var text = new NumberingContainer(knowledge.Enumerate());
			var @string = representer.RepresentText(text, language).ToString();

			// assert
			foreach (var concept in knowledge)
			{
				Assert.IsTrue(@string.Contains(concept.Name.GetValue(language)));
				Assert.IsTrue(text.GetParameters().ContainsKey(concept.ID));
			}
		}

		[Test]
		public void EnumerateOneLine()
		{
			// arrange
			var knowledge = new IKnowledge[]
			{
				"Abc".CreateConcept(),
				"Def".CreateConcept(),
				"Uvw".CreateConcept(),
				"Xyz".CreateConcept(),
			};

			var language = Language.Default;
			var representer = TextRepresenters.PlainString;

			// act
			var text = knowledge.EnumerateOneLine();
			var @string = representer.RepresentText(text, language).ToString();

			// assert
			foreach (var concept in knowledge)
			{
				Assert.IsTrue(@string.Contains(concept.Name.GetValue(language)));
				Assert.IsTrue(text.GetParameters().ContainsKey(concept.GetAnchor()));
			}
		}
	}
}
