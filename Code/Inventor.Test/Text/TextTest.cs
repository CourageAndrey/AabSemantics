using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Concepts;
using Inventor.Core.Localization;
using Inventor.Core.Text.Containers;
using Inventor.Core.Text.Primitives;

namespace Inventor.Test.Text
{
	[TestFixture]
	public class TextTest
	{
		[Test]
		public void ToStringUsesPlainRepresentation()
		{
			// arrange
			var seed = new Random(DateTime.Now.Millisecond);
			var text = new FormattedText(
				language => $"Selected random number: #NUMBER#.",
				new Dictionary<string, IKnowledge>
				{
					{ "#NUMBER#", seed.Next(1000, 9999).ToString().CreateConcept() },
				});

			// act
			string toString = text.ToString();
			string representation = TextRepresenters.PlainString.RepresentText(text, Language.Default).ToString();

			// assert
			Assert.AreEqual(representation, toString);
		}

		[Test]
		public void LineBreakHasNoParameters()
		{
			// arrange
			var text = new LineBreakText();

			// assert
			Assert.AreEqual(0, text.GetParameters().Count);
		}

		[Test]
		public void SpaceHasNoParameters()
		{
			// arrange
			var text = new SpaceText();

			// assert
			Assert.AreEqual(0, text.GetParameters().Count);
		}

		[Test]
		[TestCaseSource(nameof(getDecorators))]
		public void AllDecoratorsReturnParametersOfDecorated(ITextDecorator decorator)
		{
			// arrange & act
			var decorated = decorator.GetParameters();
			var original = decorator.InnerText.GetParameters();

			// assert
			Assert.IsTrue(decorated.SequenceEqual(original));
		}

		[Test]
		[TestCaseSource(nameof(getDecorators))]
		public void AllDecoratorsContainTextOfDecorated(ITextDecorator decorator)
		{
			// arrange & act
			string decorated = TextRepresenters.PlainString.RepresentText(decorator, Language.Default).ToString();
			string original = TextRepresenters.PlainString.RepresentText(decorator.InnerText, Language.Default).ToString();

			// assert
			Assert.IsTrue(decorated.Contains(original));
			Assert.Greater(decorated.Length, original.Length);
		}

		[Test]
		[TestCaseSource(nameof(getContainers))]
		public void AllContainersAggregateItemsParameters(ITextContainer container)
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
			Assert.IsTrue(containerParameters.SequenceEqual(originalParameters));
		}

		[Test]
		[TestCaseSource(nameof(getContainers))]
		public void AllContainersAggregateItemsText(ITextContainer container)
		{
			// arrange & act
			string containerText = TextRepresenters.PlainString.RepresentText(container, Language.Default).ToString();

			foreach (var text in container.Items)
			{
				string itemText = TextRepresenters.PlainString.RepresentText(text, Language.Default).ToString();

				// assert
				Assert.IsTrue(containerText.Contains(itemText));
			}
		}

		private static IEnumerable<ITextDecorator> getDecorators()
		{
			var createDecorated = new Func<IText>(() => new FormattedText(
				l => "Carpe diem.",
				new Dictionary<string, IKnowledge> { { "1", "ANY".CreateConcept() } }));

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

		private static IEnumerable<ITextContainer> getContainers()
		{
			var createItems = new Func<IList<IText>>(() =>
			{
				return new IText[]
				{
					new FormattedText(
						l => "A",
						new Dictionary<string, IKnowledge> { { "A", "A".CreateConcept() } }),
					new FormattedText(
						l => "B",
						new Dictionary<string, IKnowledge> { { "B", "B".CreateConcept() } }),
					new FormattedText(
						l => "C",
						new Dictionary<string, IKnowledge> { { "C", "C".CreateConcept() } }),
				};
			});

			yield return new UnstructuredContainer(createItems());
			yield return new BulletsContainer(createItems());
			yield return new NumberingContainer(createItems());
		}
	}
}