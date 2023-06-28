using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Text.Primitives;

namespace AabSemantics.Sample06.StructuredText
{
	class Program
	{
		static void Main(string[] args)
		{
			var text = new FormattedText(language => "Hello, world!")
				.Append(new LineBreakText())
				.Append(new FormattedText(language => "This text supports #0x0000# (this one does not work in standard web browsers - check Inventor.Client project instead).", new Dictionary<string, IKnowledge>
				{
					{ "#0x0000#", "hyperlinks".CreateConcept() },
				}))
				.Append(language => "Bulletin list:")
				.AppendBulletsList(new IText[]
				{
					new FormattedText(language => "Red"),
					new FormattedText(language => "Green"),
					new FormattedText(language => "Blue"),
				})
				.Append(language => "Numbering list:")
				.AppendNumberingList(new IText[]
				{
					new FormattedText(language => "First item"),
					new FormattedText(language => "Second item"),
					new FormattedText(language => "Third item"),
				})
				.Append(new FormattedText(language => "Bold text").MakeBold())
				.Append(new FormattedText(language => "Italic text").MakeItalic())
				.Append(new FormattedText(language => "Underline text").MakeUnderline())
				.Append(new FormattedText(language => "Strikeout text").MakeStrikeout())
				.Append(new FormattedText(language => "Subscript text").MakeSubscript())
				.Append(new FormattedText(language => "Superscript text").MakeSuperscript())
				.Append(new FormattedText(language => "H1 Header").MakeHeader(1))
				.Append(new FormattedText(language => "H2 Header").MakeHeader(2))
				.Append(new FormattedText(language => "H3 Header").MakeHeader(3))
				.Append(new FormattedText(language => "H4 Header").MakeHeader(4))
				.Append(new FormattedText(language => "Separate paragraph of text.").MakeParagraph());

			Console.WriteLine("This sample demonstrates inner Structured Text engine.");
			Console.WriteLine();

			Console.WriteLine("Plain text representation:");
			Console.WriteLine();

			var plainText = TextRenders.PlainString.RenderText(text, Language.Default);
			Console.Write(plainText);

			Console.WriteLine();
			Console.WriteLine(new string('=', 50));
			Console.WriteLine();

			var html = TextRenders.Html.RenderText(text, Language.Default);
			string tempHtmlFile = Path.ChangeExtension(Path.GetTempFileName(), "html");
			try
			{
				Console.WriteLine("Hypertext representation:");
				Console.WriteLine("Default web browser window should open generated HTML file...");

				File.WriteAllText(tempHtmlFile, html.ToString());

				openFile(tempHtmlFile);

				Console.WriteLine("Please, press any key after opened web browser instance is closed.");
				Console.ReadKey();
			}
			finally
			{
				if (File.Exists(tempHtmlFile))
				{
					File.Delete(tempHtmlFile);
				}
			}

			Console.WriteLine();
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey();
		}

		private static void openFile(string htmlFile)
		{
			Process.Start(new ProcessStartInfo(htmlFile)
			{
				Arguments = htmlFile,
				UseShellExecute = true,
				WorkingDirectory = Path.GetDirectoryName(htmlFile),
				FileName = htmlFile,
				Verb = "OPEN",
			});
		}
	}
}
