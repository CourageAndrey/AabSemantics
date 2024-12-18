using NUnit.Framework;

using AabSemantics.Localization;

namespace AabSemantics.IntegrationTests.Statements
{
	[TestFixture]
	public class StatementTest
	{
		[Test]
		public void GivenDifferentStatements_WhenToString_ThenResultContainsTypeAndId()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default);
			semanticNetwork.CreateCombinedTestData();

			// act & assert
			foreach (var statement in semanticNetwork.Statements)
			{
				string info = statement.ToString();

				Assert.That(info.Contains(statement.GetType().Name), Is.True);
				Assert.That(info.Contains(statement.ID), Is.True);
			}
		}

		[Test]
		public void GivenAllDescribes_WhenCall_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(Language.Default);
			semanticNetwork.CreateCombinedTestData();

			var render = TextRenders.PlainString;

			// act & assert
			foreach (var statement in semanticNetwork.Statements)
			{
				foreach (var text in new[] { statement.DescribeTrue(), statement.DescribeFalse(), statement.DescribeQuestion() })
				{
					Assert.That(text, Is.Not.Null);
					Assert.That(string.IsNullOrEmpty(render.Render(text, language).ToString()), Is.False);
				}
			}
		}
	}
}
