using NUnit.Framework;

using Inventor.Semantics;
using Inventor.Semantics.Concepts;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Modules.Classification.Statements;
using Inventor.Semantics.Statements;
using Inventor.Mathematics;
using Inventor.Processes;
using Inventor.Set;
using Inventor.Test.Sample;

namespace Inventor.Test
{
	[TestFixture]
	public class SemanticNetworkTest
	{
		[TestFixtureSetUp]
		public void InitializeModules()
		{
			new Semantics.Modules.BooleanModule().RegisterMetadata();
			new Semantics.Modules.ClassificationModule().RegisterMetadata();
			new MathematicsModule().RegisterMetadata();
			new ProcessesModule().RegisterMetadata();
			new SetModule().RegisterMetadata();
		}

		[Test]
		public void GivenConsistentSemanticNetworkWhenCheckConsistensyThenReturnEmptyText()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language).SemanticNetwork;

			// act
			var result = semanticNetwork.CheckConsistency();

			// assert
			Assert.IsTrue(result.ToString().Contains(language.Statements.Consistency.CheckOk));
		}

		[Test]
		public void GivenDuplicatedStatementWhenCheckConsistensyThenReturnDuplication()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			IConcept concept1, concept2;
			semanticNetwork.Concepts.Add(concept1 = 1.CreateConcept());
			semanticNetwork.Concepts.Add(concept2 = 2.CreateConcept());

			semanticNetwork.DeclareThat(concept1).IsAncestorOf(concept2);
			semanticNetwork.DeclareThat(concept1).IsAncestorOf(concept2);

			// act
			var result = semanticNetwork.CheckConsistency().ToString();

			// assert
			Assert.Less(language.Statements.Consistency.ErrorDuplicate.Length, result.Length);
		}

		[Test]
		public void DescribeRulesEnumeratesThem()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new TestSemanticNetwork(language).SemanticNetwork;

			// act
			var result = semanticNetwork.DescribeRules().ToString();

			// assert
			Assert.Less(4000, result.Length);
		}
	}
}
