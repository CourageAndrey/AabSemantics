using System.Collections.Generic;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Concepts;
using Inventor.Core.Localization;
using Inventor.Core.Text.Containers;
using Inventor.Core.Text.Primitives;

namespace Inventor.Test.Text
{
	[TestFixture]
	public class TextRepresentationTest
	{
		[Test]
		[TestCaseSource(typeof(TextRepresenters), nameof(TextRepresenters.All))]
		public void EnsureAllTextRepresenterFeaturesWork(IStructuredTextRepresenter representer)
		{
			// arrange
			var text = createAllFeaturesText();

			// act
			string representedText = representer.Represent(text, Language.Default).ToString();

			// assert
			for (int i = 1; i <= 21; i++) // count of used blocks within createAllFeaturesText() method
			{
				Assert.IsTrue(representedText.Contains(getText(i)));
			}
		}

		[Test]
		[TestCaseSource(typeof(TextRepresenters), nameof(TextRepresenters.All))]
		public void EnsureAnyLevelOfTextBlockIsSupported(IStructuredTextRepresenter representer)
		{
			// arrange
			string searchingToken = "QWERTY";
			IText text = new FormattedText(language => searchingToken);

			// act
			for (int i = 0; i < 10; i++)
			{
				text = new UnstructuredContainer(text);
				string representation = representer.Represent(text, Language.Default).ToString();

				// assert
				Assert.IsTrue(representation.Contains(searchingToken));
			}
		}

		private static IText createAllFeaturesText()
		{
			return
				getFormattedText(1)
				.Append(getFormattedText(2)) // call IText extension
				.Append(getFormattedText(3)) // call ITextContainer extension
				.Append(language => $"{getID(4)}", new Dictionary<string, IKnowledge>
				{
					{ getID(4), getText(4).CreateConcept() },
				})
				.AppendLineBreak()
				.AppendSpace()
				.AppendBulletsList(new IText[]
				{
					getFormattedText(5),
					getFormattedText(6),
					getFormattedText(7),
				})
				.AppendNumberingList(new IText[]
				{
					getFormattedText(8),
					getFormattedText(9),
					getFormattedText(10),
				})
				.Append(getFormattedText(11).MakeBold())
				.Append(getFormattedText(12).MakeItalic())
				.Append(getFormattedText(13).MakeUnderline())
				.Append(getFormattedText(14).MakeStrikeout())
				.Append(getFormattedText(15).MakeSubscript())
				.Append(getFormattedText(16).MakeSuperscript())
				.Append(getFormattedText(17).MakeHeader(1))
				.Append(getFormattedText(18).MakeHeader(2))
				.Append(getFormattedText(19).MakeHeader(3))
				.Append(getFormattedText(20).MakeHeader(4))
				.Append(getFormattedText(21).MakeParagraph());
		}

		private static FormattedText getFormattedText(int number)
		{
			return new FormattedText(
				language => $"+++ {getID(number)} +++",
				new Dictionary<string, IKnowledge>
				{
					{ getID(number), getText(number).CreateConcept() },
				});
		}

		private static string getID(int number)
		{
			return $"ID_{number:N3}";
		}

		private static string getText(int number)
		{
			return $"TEXT_{number:N3}";
		}
	}
}
