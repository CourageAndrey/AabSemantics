using NUnit.Framework;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Test.Sample;

namespace Inventor.Semantics.Test.Statements
{
	[TestFixture]
	public class StatementTest
	{
		[Test]
		public void ToStringContainsTypeAndId()
		{
			// arrange
			var semanticNetwork = new TestSemanticNetwork(Language.Default).SemanticNetwork;

			// act & assert
			foreach (var statement in semanticNetwork.Statements)
			{
				string info = statement.ToString();

				Assert.IsTrue(info.Contains(statement.GetType().Name));
				Assert.IsTrue(info.Contains(statement.ID));
			}
		}

		[Test]
		public void AllDescribesWorkCorrect()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language).SemanticNetwork;

			var representer = TextRepresenters.PlainString;

			// act & assert
			foreach (var statement in semanticNetwork.Statements)
			{
				foreach (var text in new[] { statement.DescribeTrue(), statement.DescribeFalse(), statement.DescribeQuestion() })
				{
					Assert.IsNotNull(text);
					Assert.False(string.IsNullOrEmpty(representer.Represent(text, language).ToString()));
				}
			}
		}
	}
}
