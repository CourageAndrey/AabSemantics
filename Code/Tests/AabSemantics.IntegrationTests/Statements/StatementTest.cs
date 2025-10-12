using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Utils;

namespace AabSemantics.IntegrationTests.Statements
{
	[TestFixture]
	public class StatementTest
	{
		[Test]
		public async Task IsStatement_CheckCyclic()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default);
			semanticNetwork.CreateCombinedTestData();
			var classifications = semanticNetwork.Statements.OfType<IsStatement>().ToList();

			// act
			var classification = classifications.First();
			bool syncResult = classification.CheckCyclic(classifications);
			bool asyncResult = await classification.CheckCyclicAsync(classifications);

			// assert
			Assert.That(asyncResult, Is.EqualTo(syncResult));
		}

		[Test]
		public async Task SignValue_HasSign()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default);
			semanticNetwork.CreateCombinedTestData();
			var statement = await semanticNetwork.Statements.OfType<SignValueStatement>().FirstAsync();

			// act
			bool syncResult = statement.CheckHasSign(semanticNetwork.Statements);
			bool asyncResult = await statement.CheckHasSignAsync(semanticNetwork.Statements);

			// assert
			Assert.That(asyncResult, Is.EqualTo(syncResult));
		}

		[Test]
		public async Task SignValue_GetValue()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default);
			var testData = semanticNetwork.CreateCombinedTestData();
			var concept = testData.Set.Vehicle_Car;
			var sign = testData.Set.Sign_MotorType;

			// act
			var syncResult = SignValueStatement.GetSignValue(semanticNetwork.Statements, concept, sign);
			var asyncResult = await SignValueStatement.GetSignValueAsync(semanticNetwork.Statements, concept, sign);

			// assert
			Assert.That(asyncResult, Is.SameAs(syncResult));
		}

		[Test]
		public async Task ComparisonSigns_Contradicts()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default);
			semanticNetwork.CreateCombinedTestData();

			// act
			bool syncResult = ComparisonSigns.IsEqualTo.Contradicts(ComparisonSigns.IsEqualTo);
			bool asyncResult = await ComparisonSigns.IsEqualTo.ContradictsAsync(ComparisonSigns.IsEqualTo);

			// assert
			Assert.That(asyncResult, Is.EqualTo(syncResult));
		}

		[Test]
		public async Task ParentChild_GetParentsOneLevel()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default);
			var testData = semanticNetwork.CreateCombinedTestData();
			var classifications = await semanticNetwork.Statements.OfType<IsStatement>().ToListAsync();
			var concept = testData.Set.Vehicle_Car;

			// act
			var syncParents1 = semanticNetwork.Statements.GetParentsOneLevel<IConcept, IsStatement>(concept);
			var asyncParents1 = await semanticNetwork.Statements.GetParentsOneLevelAsync<IConcept, IsStatement>(concept);
			var syncParents2 = classifications.GetParentsOneLevel(concept);
			var asyncParents2 = await classifications.GetParentsOneLevelAsync(concept);

			// assert
			Assert.That(syncParents1.Single(), Is.SameAs(asyncParents1.Single()));
			Assert.That(syncParents2.Single(), Is.SameAs(asyncParents2.Single()));
		}

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
