using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Text.Containers;
using AabSemantics.Text.Primitives;

namespace AabSemantics.Tests.Text
{
	[TestFixture]
	public class TextTest
	{
		[Test]
		public void GivenText_WhenToString_ThenRenderPlain()
		{
			// arrange
			var seed = new Random(DateTime.Now.Millisecond);
			var text = new FormattedText(
				language => $"Selected random number: #NUMBER#.",
				new Dictionary<string, IKnowledge>
				{
					{ "#NUMBER#", seed.Next(1000, 9999).ToString().CreateConceptByName() },
				});

			// act
			string toString = text.ToString();
			string representation = TextRenders.PlainString.RenderText(text, Language.Default).ToString();

			// assert
			Assert.That(toString, Is.EqualTo(representation));
		}

		[Test]
		public void GivenLineBreak_WhenGetParameters_ThenReturnEmpty()
		{
			// arrange
			var text = new LineBreakText();

			// assert
			Assert.That(text.GetParameters().Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenSpace_WhenGetParameters_ThenReturnEmpty()
		{
			// arrange
			var text = new SpaceText();

			// assert
			Assert.That(text.GetParameters().Count, Is.EqualTo(0));
		}

		[Test]
		[TestCaseSource(nameof(GetDecorators))]
		public void GivenAnyDecorator_WhenCheckParameters_ThenContainsDecoratedParameters(ITextDecorator decorator)
		{
			// arrange & act
			var decorated = decorator.GetParameters();
			var original = decorator.InnerText.GetParameters();

			// assert
			Assert.That(decorated.SequenceEqual(original), Is.True);
		}

		[Test]
		[TestCaseSource(nameof(GetDecorators))]
		public void GivenAnyDecorator_WhenCheckFormat_ThenContainsDecoratedFormat(ITextDecorator decorator)
		{
			// arrange & act
			string decorated = TextRenders.PlainString.RenderText(decorator, Language.Default).ToString();
			string original = TextRenders.PlainString.RenderText(decorator.InnerText, Language.Default).ToString();

			// assert
			Assert.That(decorated.Contains(original), Is.True);
			Assert.That(original.Length, Is.LessThan(decorated.Length));
		}

		[Test]
		[TestCaseSource(nameof(GetContainers))]
		public void GivenAnyContainer_WhenCheckParameters_ThenAggregateAllItemsParameters(ITextContainer container)
		{
			// arrange & act
			var containerParameters = container.GetParameters();

			var originalParameters = new Dictionary<string, IKnowledge>();
			foreach (var text in container.Items)
			{
				foreach (var parameter in text.GetParameters())
				{
					originalParameters[parameter.Key] = parameter.Value;
				}
			}

			// assert
			Assert.That(containerParameters.SequenceEqual(originalParameters), Is.True);
		}

		[Test]
		[TestCaseSource(nameof(GetContainers))]
		public void GivenAnyContainer_WhenCheckFormat_ThenThenAggregateAllItemsFormat(ITextContainer container)
		{
			// arrange & act
			string containerText = TextRenders.PlainString.RenderText(container, Language.Default).ToString();

			foreach (var text in container.Items)
			{
				string itemText = TextRenders.PlainString.RenderText(text, Language.Default).ToString();

				// assert
				Assert.That(containerText.Contains(itemText), Is.True);
			}
		}

		private static IEnumerable<ITextDecorator> GetDecorators()
		{
			var createDecorated = new Func<IText>(() => new FormattedText(
				l => "Carpe diem.",
				new Dictionary<string, IKnowledge> { { "1", "ANY".CreateConceptByName() } }));

			yield return createDecorated().MakeBold();
			yield return createDecorated().MakeItalic();
			yield return createDecorated().MakeUnderline();
			yield return createDecorated().MakeStrikeout();
			yield return createDecorated().MakeSubscript();
			yield return createDecorated().MakeSuperscript();
			yield return createDecorated().MakeHeader(1);
			yield return createDecorated().MakeHeader(2);
			yield return createDecorated().MakeHeader(3);
			yield return createDecorated().MakeHeader(4);
			yield return createDecorated().MakeParagraph();
		}

		private static IEnumerable<ITextContainer> GetContainers()
		{
			var createItems = new Func<IList<IText>>(() =>
			{
				return new IText[]
				{
					new FormattedText(
						l => "A",
						new Dictionary<string, IKnowledge> { { "A", "A".CreateConceptByName() } }),
					new FormattedText(
						l => "B",
						new Dictionary<string, IKnowledge> { { "B", "B".CreateConceptByName() } }),
					new FormattedText(
						l => "C",
						new Dictionary<string, IKnowledge> { { "C", "C".CreateConceptByName() } }),
				};
			});

			yield return new UnstructuredContainer(createItems());
			yield return new BulletsContainer(createItems());
			yield return new NumberingContainer(createItems());
		}
	}
}