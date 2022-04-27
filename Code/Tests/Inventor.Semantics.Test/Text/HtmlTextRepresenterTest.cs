using System.Text.RegularExpressions;

using NUnit.Framework;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Test.Sample;

namespace Inventor.Semantics.Test.Text
{
	[TestFixture]
	public class HtmlTextRepresenterTest
	{
		private const string ValidHtmRegex = "<(\"[^\"]*\"|'[^']*'|[^'\">])*>";

		[Test]
		public void CheckGeneratedWebPage()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language).SemanticNetwork;

			var text = semanticNetwork.DescribeRules();

			// act
			var html = TextRepresenters.Html.RepresentText(text, language).ToString();

			// assert
			Assert.IsTrue(Regex.IsMatch(html, ValidHtmRegex));
		}
	}
}
