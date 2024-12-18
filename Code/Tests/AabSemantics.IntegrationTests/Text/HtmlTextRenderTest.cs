using System.Text.RegularExpressions;

using NUnit.Framework;

using AabSemantics.Localization;

namespace AabSemantics.IntegrationTests.Text
{
	[TestFixture]
	public class HtmlTextRenderTest
	{
		private const string ValidHtmRegex = "<(\"[^\"]*\"|'[^']*'|[^'\">])*>";

		[Test]
		public void GivenHtmlRender_WhenRenderText_ThenGenerateValidHtml()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);
			semanticNetwork.CreateCombinedTestData();

			var text = semanticNetwork.DescribeRules();

			// act
			var html = TextRenders.Html.RenderText(text, language).ToString();

			// assert
			Assert.That(Regex.IsMatch(html, ValidHtmRegex), Is.True);
		}
	}
}
