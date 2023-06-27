using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Mathematics.Questions;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Concepts;
using AabSemantics.Modules.Processes.Statements;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;

namespace AabSemantics.Tests.Statements
{
	[TestFixture]
	public class StatementBuilderTest
	{
		[Test]
		public void TestBuildingHasPartStatement()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var whole = ConceptCreationHelper.CreateConcept();
			var part = ConceptCreationHelper.CreateConcept();

			// act
			var statementByConstructor = new HasPartStatement(null, whole, part);
			var statementByBuilderFromWhole = semanticNetwork.DeclareThat(whole).HasPart(part);
			var statementByBuilderFromPart = semanticNetwork.DeclareThat(part).IsPartOf(whole);

			// assert
			Assert.AreEqual(statementByConstructor, statementByBuilderFromWhole);
			Assert.AreEqual(statementByConstructor, statementByBuilderFromPart);
		}

		[Test]
		public void TestBuildingMultipleHasPartStatements()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var whole1 = ConceptCreationHelper.CreateConcept();
			var whole2 = ConceptCreationHelper.CreateConcept();
			var part1 = ConceptCreationHelper.CreateConcept();
			var part2 = ConceptCreationHelper.CreateConcept();

			// act
			var statementsByConstructorFromWhole = new List<HasPartStatement>
			{
				new HasPartStatement(null, whole1, part1),
				new HasPartStatement(null, whole1, part2),
			};
			var statementsByConstructorFromPart = new List<HasPartStatement>
			{
				new HasPartStatement(null, whole1, part1),
				new HasPartStatement(null, whole2, part1),
			};
			var statementsByBuilderFromWhole = semanticNetwork.DeclareThat(whole1).HasParts(new[] { part1, part2 });
			var statementsByBuilderFromPart = semanticNetwork.DeclareThat(part1).IsPartOf(new[] { whole1, whole2 });

			// assert
			AssertAreEqual(statementsByConstructorFromWhole, statementsByBuilderFromWhole);
			AssertAreEqual(statementsByConstructorFromPart, statementsByBuilderFromPart);
		}

		[Test]
		public void TestBuildingGroupStatement()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var area = ConceptCreationHelper.CreateConcept();
			var concept = ConceptCreationHelper.CreateConcept();

			// act
			var statementByConstructor = new GroupStatement(null, area, concept);
			var statementByBuilderFromArea = semanticNetwork.DeclareThat(area).IsSubjectAreaOf(concept);
			var statementByBuilderFromConcept = semanticNetwork.DeclareThat(concept).BelongsToSubjectArea(area);

			// assert
			Assert.AreEqual(statementByConstructor, statementByBuilderFromArea);
			Assert.AreEqual(statementByConstructor, statementByBuilderFromConcept);
		}

		[Test]
		public void TestBuildingMultipleGroupStatements()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var area1 = ConceptCreationHelper.CreateConcept();
			var area2 = ConceptCreationHelper.CreateConcept();
			var concept1 = ConceptCreationHelper.CreateConcept();
			var concept2 = ConceptCreationHelper.CreateConcept();

			// act
			var statementsByConstructorFromArea = new List<GroupStatement>
			{
				new GroupStatement(null, area1, concept1),
				new GroupStatement(null, area1, concept2),
			};
			var statementsByConstructorFromConcept = new List<GroupStatement>
			{
				new GroupStatement(null, area1, concept1),
				new GroupStatement(null, area2, concept1),
			};
			var statementsByBuilderFromArea = semanticNetwork.DeclareThat(area1).IsSubjectAreaOf(new[] { concept1, concept2 });
			var statementsByBuilderFromConcept = semanticNetwork.DeclareThat(concept1).BelongsToSubjectAreas(new[] { area1, area2 });

			// assert
			AssertAreEqual(statementsByConstructorFromArea, statementsByBuilderFromArea);
			AssertAreEqual(statementsByConstructorFromConcept, statementsByBuilderFromConcept);
		}

		[Test]
		public void TestBuildingHasSignStatement()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var concept = ConceptCreationHelper.CreateConcept();
			var sign = ConceptCreationHelper.CreateConcept();
			sign.WithAttribute(IsSignAttribute.Value);

			// act
			var statementByConstructor = new HasSignStatement(null, concept, sign);
			var statementByBuilderFromConcept = semanticNetwork.DeclareThat(concept).HasSign(sign);
			var statementByBuilderFromSign = semanticNetwork.DeclareThat(sign).IsSignOf(concept);

			// assert
			Assert.AreEqual(statementByConstructor, statementByBuilderFromConcept);
			Assert.AreEqual(statementByConstructor, statementByBuilderFromSign);
		}

		[Test]
		public void TestBuildingMultipleHasSignStatements()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var concept1 = ConceptCreationHelper.CreateConcept();
			var concept2 = ConceptCreationHelper.CreateConcept();
			var sign1 = ConceptCreationHelper.CreateConcept();
			sign1.WithAttribute(IsSignAttribute.Value);
			var sign2 = ConceptCreationHelper.CreateConcept();
			sign2.WithAttribute(IsSignAttribute.Value);

			// act
			var statementsByConstructorFromConcept = new List<HasSignStatement>
			{
				new HasSignStatement(null, concept1, sign1),
				new HasSignStatement(null, concept1, sign2),
			};
			var statementsByConstructorFromSign = new List<HasSignStatement>
			{
				new HasSignStatement(null, concept1, sign1),
				new HasSignStatement(null, concept2, sign1),
			};
			var statementsByBuilderFromConcept = semanticNetwork.DeclareThat(concept1).HasSigns(new[] { sign1, sign2 });
			var statementsByBuilderFromSign = semanticNetwork.DeclareThat(sign1).IsSignOf(new[] { concept1, concept2 });

			// assert
			AssertAreEqual(statementsByConstructorFromConcept, statementsByBuilderFromConcept);
			AssertAreEqual(statementsByConstructorFromSign, statementsByBuilderFromSign);
		}

		[Test]
		public void TestBuildingIsStatement()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var ancestor = ConceptCreationHelper.CreateConcept();
			var descendant = ConceptCreationHelper.CreateConcept();

			// act
			var statementByConstructor = new IsStatement(null, ancestor, descendant);
			var statementByBuilderFromAncestor = semanticNetwork.DeclareThat(ancestor).IsAncestorOf(descendant);
			var statementByBuilderFromDescendant = semanticNetwork.DeclareThat(descendant).IsDescendantOf(ancestor);

			// assert
			Assert.AreEqual(statementByConstructor, statementByBuilderFromAncestor);
			Assert.AreEqual(statementByConstructor, statementByBuilderFromDescendant);
		}

		[Test]
		public void TestBuildingMultipleIsStatements()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var ancestor = ConceptCreationHelper.CreateConcept();
			var ancestor1 = ConceptCreationHelper.CreateConcept();
			var ancestor2 = ConceptCreationHelper.CreateConcept();
			var ancestor3 = ConceptCreationHelper.CreateConcept();
			var descendant = ConceptCreationHelper.CreateConcept();
			var descendant1 = ConceptCreationHelper.CreateConcept();
			var descendant2 = ConceptCreationHelper.CreateConcept();
			var descendant3 = ConceptCreationHelper.CreateConcept();

			// act
			var statementsByBuilderFromAncestor = semanticNetwork.DeclareThat(ancestor).IsAncestorOf(new[] { descendant1, descendant2, descendant3 });
			var statementsByBuilderFromDescendant = semanticNetwork.DeclareThat(descendant).IsDescendantOf(new[] { ancestor1, ancestor2, ancestor3 });

			// assert
			Assert.AreEqual(6, semanticNetwork.Statements.Count);
			Assert.IsTrue(semanticNetwork.Statements.All(s => s is IsStatement));
			Assert.AreEqual(3, statementsByBuilderFromAncestor.Count);
			Assert.IsTrue(statementsByBuilderFromAncestor.All(s => s.Ancestor == ancestor));
			Assert.AreEqual(3, statementsByBuilderFromDescendant.Count);
			Assert.IsTrue(statementsByBuilderFromDescendant.All(s => s.Descendant == descendant));
		}

		[Test]
		public void TestBuildingSignValueStatement()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var concept = ConceptCreationHelper.CreateConcept();
			var sign = ConceptCreationHelper.CreateConcept();
			sign.WithAttribute(IsSignAttribute.Value);
			var value = ConceptCreationHelper.CreateConcept();
			value.WithAttribute(IsValueAttribute.Value);

			// act
			var statementByConstructor = new SignValueStatement(null, concept, sign, value);
			var statementByBuilderFromConcept = semanticNetwork.DeclareThat(concept).HasSignValue(sign, value);
			var statementByBuilderFromValue = semanticNetwork.DeclareThat(value).IsSignValue(concept, sign);

			// assert
			Assert.AreEqual(statementByConstructor, statementByBuilderFromConcept);
			Assert.AreEqual(statementByConstructor, statementByBuilderFromValue);
		}

		[Test]
		public void TestBuildingComparisonStatement()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var leftValue = ConceptCreationHelper.CreateConcept();
			leftValue.WithAttribute(IsValueAttribute.Value);
			var rightValue = ConceptCreationHelper.CreateConcept();
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
		public void TestBuildingMultipleComparisonStatement()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var leftValue = ConceptCreationHelper.CreateConcept();
			leftValue.WithAttribute(IsValueAttribute.Value);
			var rightValues = new[]
			{
				ConceptCreationHelper.CreateConcept(),
				ConceptCreationHelper.CreateConcept(),
				ConceptCreationHelper.CreateConcept(),
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
		[TestCaseSource(nameof(getChainComparisons))]
		public void TestBuildingChainComparisons(Action<ISemanticNetwork, IEnumerable<IConcept>> definitionMethod, IConcept comparisonSign)
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			const int count = 10;
			var numbers = new List<IConcept>();
			for (int i = 1; i <= count; i++)
			{
				string n = i.ToString();
				IConcept number = n.CreateConcept().WithAttribute(IsValueAttribute.Value);
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

		private static IEnumerable<object[]> getChainComparisons()
		{
			yield return new object[] { new Action<ISemanticNetwork, IEnumerable<IConcept>>((semanticNetwork, numbers) => semanticNetwork.DefineAscendingSequence(numbers)), ComparisonSigns.IsLessThan };
			yield return new object[] { new Action<ISemanticNetwork, IEnumerable<IConcept>>((semanticNetwork, numbers) => semanticNetwork.DefineDescendingSequence(numbers)), ComparisonSigns.IsGreaterThan };
			yield return new object[] { new Action<ISemanticNetwork, IEnumerable<IConcept>>((semanticNetwork, numbers) => semanticNetwork.DefineNotAscendingSequence(numbers)), ComparisonSigns.IsGreaterThanOrEqualTo };
			yield return new object[] { new Action<ISemanticNetwork, IEnumerable<IConcept>>((semanticNetwork, numbers) => semanticNetwork.DefineNotDescendingSequence(numbers)), ComparisonSigns.IsLessThanOrEqualTo };
		}

		[Test]
		public void TestBuildingProcessesStatement()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var processA = ConceptCreationHelper.CreateConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var processB = ConceptCreationHelper.CreateConcept();
			processB.WithAttribute(IsProcessAttribute.Value);

			var statementsByConstructor = new List<ProcessesStatement>();
			var statementsByBuilder = new List<ProcessesStatement>();

			// act
			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.StartsAfterOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsAfterOtherStarted(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.StartsWhenOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsWhenOtherStarted(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.StartsBeforeOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsBeforeOtherStarted(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesAfterOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesAfterOtherStarted(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesWhenOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesWhenOtherStarted(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesBeforeOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesBeforeOtherStarted(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.StartsAfterOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsAfterOtherFinished(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.StartsWhenOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsWhenOtherFinished(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.StartsBeforeOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsBeforeOtherFinished(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesAfterOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesAfterOtherFinished(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesWhenOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesWhenOtherFinished(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesBeforeOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesBeforeOtherFinished(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.Causes));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).Causes(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.IsCausedBy));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).IsCausedBy(processB));

			statementsByConstructor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.SimultaneousWith));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).SimultaneousWith(processB));

			// assert
			for (int s = 0; s < SequenceSigns.All.Count; s++)
			{
				Assert.AreEqual(statementsByConstructor[s], statementsByBuilder[s]);
			}
		}

		[Test]
		public void TestBuildingMultipleProcessesStatement()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var processA = ConceptCreationHelper.CreateConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var processesB = new[]
			{
				ConceptCreationHelper.CreateConcept(),
				ConceptCreationHelper.CreateConcept(),
				ConceptCreationHelper.CreateConcept(),
			};
			foreach (var processB in processesB)
			{
				processB.WithAttribute(IsProcessAttribute.Value);
			}

			var statementsByConstructor = new List<List<ProcessesStatement>>();
			var statementsByBuilder = new List<List<ProcessesStatement>>();

			// act
			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.StartsAfterOtherStarted)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsAfterOthersStarted(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.StartsWhenOtherStarted)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsWhenOthersStarted(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.StartsBeforeOtherStarted)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsBeforeOthersStarted(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesAfterOtherStarted)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesAfterOthersStarted(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesWhenOtherStarted)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesWhenOthersStarted(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesBeforeOtherStarted)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesBeforeOthersStarted(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.StartsAfterOtherFinished)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsAfterOthersFinished(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.StartsWhenOtherFinished)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsWhenOthersFinished(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.StartsBeforeOtherFinished)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsBeforeOthersFinished(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesAfterOtherFinished)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesAfterOthersFinished(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesWhenOtherFinished)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesWhenOthersFinished(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesBeforeOtherFinished)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesBeforeOthersFinished(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.Causes)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).Causes(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.IsCausedBy)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).IsCausedBy(processesB));

			statementsByConstructor.Add(processesB.Select(processB => new ProcessesStatement(null, processA, processB, SequenceSigns.SimultaneousWith)).ToList());
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).SimultaneousWith(processesB));

			// assert
			for (int s = 0; s < SequenceSigns.All.Count; s++)
			{
				AssertAreEqual(statementsByConstructor[s], statementsByBuilder[s]);
			}
		}

		[Test]
		public void ImpossibleToCreateWithoutSemanticNetwork()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new StatementBuilder(null, 1.CreateConcept()));
		}

		[Test]
		public void ImpossibleToCreateWithoutConcept()
		{
			// act & assert
			Assert.Throws<ArgumentNullException>(() => new StatementBuilder(new SemanticNetwork(Language.Default), null));
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
