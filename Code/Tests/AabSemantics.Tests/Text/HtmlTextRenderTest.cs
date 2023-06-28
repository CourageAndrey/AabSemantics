using System.Text.RegularExpressions;

using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Test.Sample;

namespace AabSemantics.Tests.Text
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
			semanticNetwork.CreateSetTestData();
			semanticNetwork.CreateMathematicsTestData();
			semanticNetwork.CreateProcessesTestData();

			var text = semanticNetwork.DescribeRules();

			// act
			var html = TextRenders.Html.RenderText(text, language).ToString();

			// assert
			Assert.IsTrue(Regex.IsMatch(html, ValidHtmRegex));
		}
	}
}
