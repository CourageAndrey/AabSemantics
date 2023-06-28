using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Text.Containers;

namespace AabSemantics.Tests.Text
{
	[TestFixture]
	public class EnumerationTest
	{
		[Test]
		public void GivenManyConcepts_WhenEnumerate_ThenCreateMultilineList()
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
			var render = TextRenders.PlainString;

			// act
			var text = new NumberingContainer(knowledge.Enumerate());
			var @string = render.RenderText(text, language).ToString();

			// assert
			foreach (var concept in knowledge)
			{
				Assert.IsTrue(@string.Contains(concept.Name.GetValue(language)));
				Assert.IsTrue(text.GetParameters().ContainsKey(concept.ID));
			}
		}

		[Test]
		public void Given_WhenEnumerateOneLine_ThenCreateSingleLineList()
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
			var render = TextRenders.PlainString;

			// act
			var text = knowledge.EnumerateOneLine();
			var @string = render.RenderText(text, language).ToString();

			// assert
			foreach (var concept in knowledge)
			{
				Assert.IsTrue(@string.Contains(concept.Name.GetValue(language)));
				Assert.IsTrue(text.GetParameters().ContainsKey(concept.GetAnchor()));
			}
		}
	}
}
