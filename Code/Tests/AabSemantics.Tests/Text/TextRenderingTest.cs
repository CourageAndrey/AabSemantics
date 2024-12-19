﻿using System;
using System.Collections.Generic;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Text.Containers;
using AabSemantics.Text.Primitives;

namespace AabSemantics.Tests.Text
{
	[TestFixture]
	public class TextRenderingTest
	{
		[Test]
		[TestCaseSource(typeof(TextRenders), nameof(TextRenders.All))]
		public void GivenComplicatedText_WhenRender_ThenSucceed(IStructuredTextRender render)
		{
			// arrange
			var text = CreateAllFeaturesText();

			// act
			string renderedText = render.Render(text, Language.Default).ToString();

			// assert
			for (int i = 1; i <= 21; i++) // count of used blocks within CreateAllFeaturesText() method
			{
				Assert.That(renderedText.Contains(GetText(i)), Is.True);
			}
		}

		[Test]
		[TestCaseSource(typeof(TextRenders), nameof(TextRenders.All))]
		public void GivenMultipleLevelsText_WhenRender_ThenSucceed(IStructuredTextRender render)
		{
			// arrange
			string searchingToken = "QWERTY";
			IText text = new FormattedText(language => searchingToken);

			// act
			for (int i = 0; i < 10; i++)
			{
				text = new UnstructuredContainer(text);
				string representation = render.Render(text, Language.Default).ToString();

				// assert
				Assert.That(representation.Contains(searchingToken), Is.True);
			}
		}

		[Test]
		public void GivenUnsupportedText_WhenTryToRender_ThenFail()
		{
			// arrange
			IText text = new TestText();

			// act & arrange
			Assert.Throws<NotSupportedException>(() => TextRenders.PlainString.Render(text, Language.Default));
		}

		private static IText CreateAllFeaturesText()
		{
			return
				GetFormattedText(1)
				.Append(GetFormattedText(2)) // call IText extension
				.Append(GetFormattedText(3)) // call ITextContainer extension
				.Append(language => $"{GetID(4)}", new Dictionary<string, IKnowledge>
				{
					{ GetID(4), GetText(4).CreateConceptByName() },
				})
				.AppendLineBreak()
				.AppendSpace()
				.AppendBulletsList(new IText[]
				{
					GetFormattedText(5),
					GetFormattedText(6),
					GetFormattedText(7),
				})
				.AppendNumberingList(new IText[]
				{
					GetFormattedText(8),
					GetFormattedText(9),
					GetFormattedText(10),
				})
				.Append(GetFormattedText(11).MakeBold())
				.Append(GetFormattedText(12).MakeItalic())
				.Append(GetFormattedText(13).MakeUnderline())
				.Append(GetFormattedText(14).MakeStrikeout())
				.Append(GetFormattedText(15).MakeSubscript())
				.Append(GetFormattedText(16).MakeSuperscript())
				.Append(GetFormattedText(17).MakeHeader(1))
				.Append(GetFormattedText(18).MakeHeader(2))
				.Append(GetFormattedText(19).MakeHeader(3))
				.Append(GetFormattedText(20).MakeHeader(4))
				.Append(GetFormattedText(21).MakeParagraph());
		}

		private static FormattedText GetFormattedText(int number)
		{
			return new FormattedText(
				language => $"+++ {GetID(number)} +++",
				new Dictionary<string, IKnowledge>
				{
					{ GetID(number), GetText(number).CreateConceptByName() },
				});
		}

		private static string GetID(int number)
		{
			return $"ID_{number:N3}";
		}

		private static string GetText(int number)
		{
			return $"TEXT_{number:N3}";
		}

		private class TestText : IText
		{
			public IDictionary<string, IKnowledge> GetParameters()
			{
				return new Dictionary<string, IKnowledge>();
			}
		}
	}
}
