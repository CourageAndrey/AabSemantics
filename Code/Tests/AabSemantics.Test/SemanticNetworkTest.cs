using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Mathematics;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.Modules.Processes;
using AabSemantics.Modules.Set;
using AabSemantics.Statements;
using AabSemantics.Test.Sample;

namespace AabSemantics.Test
{
	[TestFixture]
	public class SemanticNetworkTest
	{
		[OneTimeSetUp]
		public void InitializeModules()
		{
			new Modules.Boolean.BooleanModule().RegisterMetadata();
			new Modules.Classification.ClassificationModule().RegisterMetadata();
			new MathematicsModule().RegisterMetadata();
			new ProcessesModule().RegisterMetadata();
			new SetModule().RegisterMetadata();
		}

		[Test]
		public void GivenConsistentSemanticNetworkWhenCheckConsistencyThenReturnEmptyText()
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
		public void GivenDuplicatedStatementWhenCheckConsistencyThenReturnDuplication()
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

		[Test]
		public void WhenRemoveConceptThenAlsoRemoveRelatedStatements()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			IConcept concept1, concept2;
			semanticNetwork.Concepts.Add(concept1 = 1.CreateConcept().WithAttribute(IsValueAttribute.Value));
			semanticNetwork.Concepts.Add(concept2 = 2.CreateConcept().WithAttribute(IsValueAttribute.Value));

			const int statementsCount = 10;
			for (int i = 0; i < statementsCount; i++)
			{
				semanticNetwork.DeclareThat(concept1).IsLessThan(concept2);
			}

			// act & assert
			Assert.AreEqual(statementsCount, semanticNetwork.Statements.Count);
			semanticNetwork.Concepts.Remove(concept1);
			Assert.AreEqual(0, semanticNetwork.Statements.Count);
		}

		[Test]
		public void ImpossibleToRemoveSystemStatements()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			IConcept concept1, concept2;
			semanticNetwork.Concepts.Add(concept1 = 1.CreateConcept().WithAttribute(IsValueAttribute.Value));
			semanticNetwork.Concepts.Add(concept2 = 2.CreateConcept().WithAttribute(IsValueAttribute.Value));

			var comparison = semanticNetwork.DeclareThat(concept1).IsLessThan(concept2);
			comparison.Context = semanticNetwork.Context.Parent;

			// act & assert
			Assert.IsTrue(semanticNetwork.Statements.Contains(comparison));
			semanticNetwork.Statements.Remove(comparison);
			Assert.IsTrue(semanticNetwork.Statements.Contains(comparison));

			comparison.Context = semanticNetwork.Context;
			semanticNetwork.Statements.Remove(comparison);
			Assert.IsFalse(semanticNetwork.Statements.Contains(comparison));
		}

		[Test]
		public void CheckToString()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default);

			// act
			string toString = semanticNetwork.ToString();

			// assert
			Assert.IsTrue(toString.Contains(semanticNetwork.Name.GetValue(Language.Default)));
		}
	}
}
