using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Semantics.Concepts;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Modules.Classification.Statements;
using Inventor.Semantics.Statements;
using Inventor.Semantics.Mathematics.Concepts;
using Inventor.Semantics.Mathematics.Statements;
using Inventor.Semantics.Modules.Boolean.Attributes;
using Inventor.Semantics.Processes.Attributes;
using Inventor.Semantics.Processes.Concepts;
using Inventor.Semantics.Processes.Statements;
using Inventor.Semantics.Set.Attributes;
using Inventor.Semantics.Set.Statements;

namespace Inventor.Semantics.Test.Statements
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

			var statementsByConstuctor = new List<ComparisonStatement>();
			var statementsByBuilder = new List<ComparisonStatement>();

			// act
			statementsByConstuctor.Add(new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsEqualTo));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsEqualTo(rightValue));

			statementsByConstuctor.Add(new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsNotEqualTo));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsNotEqualTo(rightValue));

			statementsByConstuctor.Add(new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsGreaterThan));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsGreaterThan(rightValue));

			statementsByConstuctor.Add(new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsGreaterThanOrEqualTo));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsGreaterThanOrEqualTo(rightValue));

			statementsByConstuctor.Add(new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsLessThan));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsLessThan(rightValue));

			statementsByConstuctor.Add(new ComparisonStatement(null, leftValue, rightValue, ComparisonSigns.IsLessThanOrEqualTo));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsLessThanOrEqualTo(rightValue));

			// assert
			for (int s = 0; s < ComparisonSigns.All.Count; s++)
			{
				Assert.AreEqual(statementsByConstuctor[s], statementsByBuilder[s]);
			}
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

			var statementsByConstuctor = new List<ProcessesStatement>();
			var statementsByBuilder = new List<ProcessesStatement>();

			// act
			statementsByConstuctor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.StartsAfterOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsAfterOtherStarted(processB));

			statementsByConstuctor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.StartsWhenOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsWhenOtherStarted(processB));

			statementsByConstuctor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.StartsBeforeOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsBeforeOtherStarted(processB));

			statementsByConstuctor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesAfterOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesAfterOtherStarted(processB));

			statementsByConstuctor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesWhenOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesWhenOtherStarted(processB));

			statementsByConstuctor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesBeforeOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesBeforeOtherStarted(processB));

			statementsByConstuctor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.StartsAfterOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsAfterOtherFinished(processB));

			statementsByConstuctor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.StartsWhenOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsWhenOtherFinished(processB));

			statementsByConstuctor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.StartsBeforeOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsBeforeOtherFinished(processB));

			statementsByConstuctor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesAfterOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesAfterOtherFinished(processB));

			statementsByConstuctor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesWhenOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesWhenOtherFinished(processB));

			statementsByConstuctor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.FinishesBeforeOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesBeforeOtherFinished(processB));

			statementsByConstuctor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.Causes));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).Causes(processB));

			statementsByConstuctor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.IsCausedBy));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).IsCausedBy(processB));

			statementsByConstuctor.Add(new ProcessesStatement(null, processA, processB, SequenceSigns.SimultaneousWith));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).SimultaneousWith(processB));

			// assert
			for (int s = 0; s < SequenceSigns.All.Count; s++)
			{
				Assert.AreEqual(statementsByConstuctor[s], statementsByBuilder[s]);
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
	}
}
