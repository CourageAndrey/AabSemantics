using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Modules.Mathematics.Tests;
using AabSemantics.Modules.Processes.Tests;
using AabSemantics.Modules.Set.Tests;

namespace AabSemantics.Tests.Statements
{
	[TestFixture]
	public class StatementTest
	{
		[Test]
		public void GivenDifferentStatements_WhenToString_ThenResultContainsTypeAndId()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default);
			semanticNetwork.CreateSetTestData();
			semanticNetwork.CreateMathematicsTestData();
			semanticNetwork.CreateProcessesTestData();

			// act & assert
			foreach (var statement in semanticNetwork.Statements)
			{
				string info = statement.ToString();

				Assert.IsTrue(info.Contains(statement.GetType().Name));
				Assert.IsTrue(info.Contains(statement.ID));
			}
		}

		[Test]
		public void GivenAllDescribes_WhenCall_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(Language.Default);
			semanticNetwork.CreateSetTestData();
			semanticNetwork.CreateMathematicsTestData();
			semanticNetwork.CreateProcessesTestData();

			var render = TextRenders.PlainString;

			// act & assert
			foreach (var statement in semanticNetwork.Statements)
			{
				foreach (var text in new[] { statement.DescribeTrue(), statement.DescribeFalse(), statement.DescribeQuestion() })
				{
					Assert.IsNotNull(text);
					Assert.False(string.IsNullOrEmpty(render.Render(text, language).ToString()));
				}
			}
		}
	}
}
