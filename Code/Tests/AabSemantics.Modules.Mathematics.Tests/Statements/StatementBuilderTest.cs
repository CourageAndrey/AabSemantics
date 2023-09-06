using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Mathematics.Questions;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Mathematics.Tests.Statements
{
	[TestFixture]
	public class StatementBuilderTest
	{
		[Test]
		public void GivenComparisonStatement_WhenDeclare_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var leftValue = ConceptCreationHelper.CreateEmptyConcept();
			leftValue.WithAttribute(IsValueAttribute.Value);
			var rightValue = ConceptCreationHelper.CreateEmptyConcept();
			rightValue.WithAttribute(IsValueAttribute.Value);

			var statementsByConstructor = new List<ComparisonStatement>();
			var statementsByBuilder = new List<ComparisonStatement>();

			// act
			statementsByConstructor.Add(new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsEqualTo));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsEqualTo(rightValue));

			statementsByConstructor.Add(new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsNotEqualTo));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsNotEqualTo(rightValue));

			statementsByConstructor.Add(new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsGreaterThan));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsGreaterThan(rightValue));

			statementsByConstructor.Add(new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsGreaterThanOrEqualTo));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsGreaterThanOrEqualTo(rightValue));

			statementsByConstructor.Add(new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsLessThan));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsLessThan(rightValue));

			statementsByConstructor.Add(new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsLessThanOrEqualTo));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsLessThanOrEqualTo(rightValue));

			// assert
			for (int s = 0; s < ComparisonSigns.All.Count; s++)
			{
				Assert.AreEqual(statementsByConstructor[s], statementsByBuilder[s]);
			}
		}

		[Test]
		public void GivenMultipleComparisonStatements_WhenDeclare_ThenSucceed()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var leftValue = ConceptCreationHelper.CreateEmptyConcept();
			leftValue.WithAttribute(IsValueAttribute.Value);
			var rightValues = new[]
			{
				ConceptCreationHelper.CreateEmptyConcept(),
				ConceptCreationHelper.CreateEmptyConcept(),
				ConceptCreationHelper.CreateEmptyConcept(),
			};
			foreach (var rightValue in rightValues)
			{
				rightValue.WithAttribute(IsValueAttribute.Value);
			}

			var statementsByConstructor = new List<List<ComparisonStatement>>();
			var statementsByBuilder = new List<List<ComparisonStatement>>();

			// act
			statementsByConstructor.Add(rightValues.Select(rightValue => new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsEqualTo)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsEqualTo(rightValues));

			statementsByConstructor.Add(rightValues.Select(rightValue => new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsNotEqualTo)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsNotEqualTo(rightValues));

			statementsByConstructor.Add(rightValues.Select(rightValue => new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsGreaterThan)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsGreaterThan(rightValues));

			statementsByConstructor.Add(rightValues.Select(rightValue => new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsGreaterThanOrEqualTo)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsGreaterThanOrEqualTo(rightValues));

			statementsByConstructor.Add(rightValues.Select(rightValue => new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsLessThan)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsLessThan(rightValues));

			statementsByConstructor.Add(rightValues.Select(rightValue => new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsLessThanOrEqualTo)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsLessThanOrEqualTo(rightValues));

			// assert
			for (int s = 0; s < ComparisonSigns.All.Count; s++)
			{
				AssertAreEqual(statementsByConstructor[s], statementsByBuilder[s]);
			}
		}

		[Test]
		[TestCaseSource(nameof(GetChainComparisons))]
		public void Given_WhenDeclare_ThenSucceed(Action<ISemanticNetwork, IEnumerable<IConcept>> definitionMethod, IConcept comparisonSign)
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			const int count = 10;
			var numbers = new List<IConcept>();
			for (int i = 1; i <= count; i++)
			{
				string n = i.ToString();
				IConcept number = n.CreateConceptByName().WithAttribute(IsValueAttribute.Value);
				numbers.Add(number);
				semanticNetwork.Concepts.Add(number);
			}

			// smoke assert
			Assert.AreEqual(0, semanticNetwork.Statements.Count);

			// act
			definitionMethod(semanticNetwork, numbers);
			var answer = (StatementAnswer) semanticNetwork.Ask().HowCompared(numbers.First(), numbers.Last());

			// assert
			Assert.AreEqual(count - 1, semanticNetwork.Statements.Count);
			Assert.AreEqual(count - 1, semanticNetwork.Statements.OfType<ComparisonStatement>().Count(comparison => comparison.ComparisonSign == comparisonSign));
			Assert.AreSame(comparisonSign, ((ComparisonStatement) answer.Result).ComparisonSign);
		}

		private static IEnumerable<object[]> GetChainComparisons()
		{
			yield return new object[] { new Action<ISemanticNetwork, IEnumerable<IConcept>>((semanticNetwork, numbers) => semanticNetwork.DefineAscendingSequence(numbers)), ComparisonSigns.IsLessThan };
			yield return new object[] { new Action<ISemanticNetwork, IEnumerable<IConcept>>((semanticNetwork, numbers) => semanticNetwork.DefineDescendingSequence(numbers)), ComparisonSigns.IsGreaterThan };
			yield return new object[] { new Action<ISemanticNetwork, IEnumerable<IConcept>>((semanticNetwork, numbers) => semanticNetwork.DefineNotAscendingSequence(numbers)), ComparisonSigns.IsGreaterThanOrEqualTo };
			yield return new object[] { new Action<ISemanticNetwork, IEnumerable<IConcept>>((semanticNetwork, numbers) => semanticNetwork.DefineNotDescendingSequence(numbers)), ComparisonSigns.IsLessThanOrEqualTo };
		}

		private static void AssertAreEqual<T>(ICollection<T> sequence1, ICollection<T> sequence2)
			where T : IEquatable<T>
		{
			Assert.AreEqual(sequence1.Count, sequence1.Count);
			foreach (var item in sequence1)
			{
				Assert.IsTrue(sequence2.Contains(item));
			}
		}
	}
}
