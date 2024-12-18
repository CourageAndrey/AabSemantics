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
				"Abc".CreateConceptByName(),
				"Def".CreateConceptByName(),
				"Uvw".CreateConceptByName(),
				"Xyz".CreateConceptByName(),
			};

			var language = Language.Default;
			var render = TextRenders.PlainString;

			// act
			var text = new NumberingContainer(knowledge.Enumerate());
			var @string = render.RenderText(text, language).ToString();

			// assert
			foreach (var concept in knowledge)
			{
				Assert.That(@string.Contains(concept.Name.GetValue(language)), Is.True);
				Assert.That(text.GetParameters().ContainsKey(concept.ID), Is.True);
			}
		}

		[Test]
		public void Given_WhenEnumerateOneLine_ThenCreateSingleLineList()
		{
			// arrange
			var knowledge = new IKnowledge[]
			{
				"Abc".CreateConceptByName(),
				"Def".CreateConceptByName(),
				"Uvw".CreateConceptByName(),
				"Xyz".CreateConceptByName(),
			};

			var language = Language.Default;
			var render = TextRenders.PlainString;

			// act
			var text = knowledge.EnumerateOneLine();
			var @string = render.RenderText(text, language).ToString();

			// assert
			foreach (var concept in knowledge)
			{
				Assert.That(@string.Contains(concept.Name.GetValue(language)), Is.True);
				Assert.That(text.GetParameters().ContainsKey(concept.GetAnchor()), Is.True);
			}
		}
	}
}
