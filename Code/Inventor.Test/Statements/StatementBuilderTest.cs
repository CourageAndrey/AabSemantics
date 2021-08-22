using System.Collections.Generic;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Test.Statements
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

			var whole = TestHelper.CreateConcept();
			var part = TestHelper.CreateConcept();

			// act
			var statementByConstuctor = new HasPartStatement(whole, part);
			var statementByBuilderFromWhole = semanticNetwork.DeclareThat(whole).HasPart(part);
			var statementByBuilderFromPart = semanticNetwork.DeclareThat(part).IsPartOf(whole);

			// assert
			Assert.AreEqual(statementByConstuctor, statementByBuilderFromWhole);
			Assert.AreEqual(statementByConstuctor, statementByBuilderFromPart);
		}

		[Test]
		public void TestBuildingGroupStatement()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var area = TestHelper.CreateConcept();
			var concept = TestHelper.CreateConcept();

			// act
			var statementByConstuctor = new GroupStatement(area, concept);
			var statementByBuilderFromArea = semanticNetwork.DeclareThat(area).IsSubjectAreaOf(concept);
			var statementByBuilderFromConcept = semanticNetwork.DeclareThat(concept).BelongsToSubjectArea(area);

			// assert
			Assert.AreEqual(statementByConstuctor, statementByBuilderFromArea);
			Assert.AreEqual(statementByConstuctor, statementByBuilderFromConcept);
		}

		[Test]
		public void TestBuildingHasSignStatement()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var concept = TestHelper.CreateConcept();
			var sign = TestHelper.CreateConcept();
			sign.WithAttribute(IsSignAttribute.Value);

			// act
			var statementByConstuctor = new HasSignStatement(concept, sign);
			var statementByBuilderFromConcept = semanticNetwork.DeclareThat(concept).HasSign(sign);
			var statementByBuilderFromSign = semanticNetwork.DeclareThat(sign).IsSignOf(concept);

			// assert
			Assert.AreEqual(statementByConstuctor, statementByBuilderFromConcept);
			Assert.AreEqual(statementByConstuctor, statementByBuilderFromSign);
		}

		[Test]
		public void TestBuildingIsStatement()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var ancestor = TestHelper.CreateConcept();
			var descendant = TestHelper.CreateConcept();

			// act
			var statementByConstuctor = new IsStatement(ancestor, descendant);
			var statementByBuilderFromAncestor = semanticNetwork.DeclareThat(ancestor).IsAncestorOf(descendant);
			var statementByBuilderFromDescendant = semanticNetwork.DeclareThat(descendant).IsDescendantOf(ancestor);

			// assert
			Assert.AreEqual(statementByConstuctor, statementByBuilderFromAncestor);
			Assert.AreEqual(statementByConstuctor, statementByBuilderFromDescendant);
		}

		[Test]
		public void TestBuildingSignValueStatement()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var concept = TestHelper.CreateConcept();
			var sign = TestHelper.CreateConcept();
			sign.WithAttribute(IsSignAttribute.Value);
			var value = TestHelper.CreateConcept();
			value.WithAttribute(IsValueAttribute.Value);

			// act
			var statementByConstuctor = new SignValueStatement(concept, sign, value);
			var statementByBuilderFromConcept = semanticNetwork.DeclareThat(concept).HasSignValue(sign, value);
			var statementByBuilderFromValue = semanticNetwork.DeclareThat(value).IsSignValue(concept, sign);

			// assert
			Assert.AreEqual(statementByConstuctor, statementByBuilderFromConcept);
			Assert.AreEqual(statementByConstuctor, statementByBuilderFromValue);
		}

		[Test]
		public void TestBuildingComparisonStatement()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language);

			var leftValue = TestHelper.CreateConcept();
			leftValue.WithAttribute(IsValueAttribute.Value);
			var rightValue = TestHelper.CreateConcept();
			rightValue.WithAttribute(IsValueAttribute.Value);

			var statementsByConstuctor = new List<ComparisonStatement>();
			var statementsByBuilder = new List<ComparisonStatement>();

			// act
			statementsByConstuctor.Add(new ComparisonStatement(leftValue, rightValue, ComparisonSigns.IsEqualTo));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsEqualTo(rightValue));

			statementsByConstuctor.Add(new ComparisonStatement(leftValue, rightValue, ComparisonSigns.IsNotEqualTo));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsNotEqualTo(rightValue));

			statementsByConstuctor.Add(new ComparisonStatement(leftValue, rightValue, ComparisonSigns.IsGreaterThan));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsGreaterThan(rightValue));

			statementsByConstuctor.Add(new ComparisonStatement(leftValue, rightValue, ComparisonSigns.IsGreaterThanOrEqualTo));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsGreaterThanOrEqualTo(rightValue));

			statementsByConstuctor.Add(new ComparisonStatement(leftValue, rightValue, ComparisonSigns.IsLessThan));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(leftValue).IsLessThan(rightValue));

			statementsByConstuctor.Add(new ComparisonStatement(leftValue, rightValue, ComparisonSigns.IsLessThanOrEqualTo));
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

			var processA = TestHelper.CreateConcept();
			processA.WithAttribute(IsProcessAttribute.Value);
			var processB = TestHelper.CreateConcept();
			processB.WithAttribute(IsProcessAttribute.Value);

			var statementsByConstuctor = new List<ProcessesStatement>();
			var statementsByBuilder = new List<ProcessesStatement>();

			// act
			statementsByConstuctor.Add(new ProcessesStatement(processA, processB, SequenceSigns.StartsAfterOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsAfterOtherStarted(processB));

			statementsByConstuctor.Add(new ProcessesStatement(processA, processB, SequenceSigns.StartsWhenOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsWhenOtherStarted(processB));

			statementsByConstuctor.Add(new ProcessesStatement(processA, processB, SequenceSigns.StartsBeforeOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsBeforeOtherStarted(processB));

			statementsByConstuctor.Add(new ProcessesStatement(processA, processB, SequenceSigns.FinishesAfterOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesAfterOtherStarted(processB));

			statementsByConstuctor.Add(new ProcessesStatement(processA, processB, SequenceSigns.FinishesWhenOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesWhenOtherStarted(processB));

			statementsByConstuctor.Add(new ProcessesStatement(processA, processB, SequenceSigns.FinishesBeforeOtherStarted));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesBeforeOtherStarted(processB));

			statementsByConstuctor.Add(new ProcessesStatement(processA, processB, SequenceSigns.StartsAfterOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsAfterOtherFinished(processB));

			statementsByConstuctor.Add(new ProcessesStatement(processA, processB, SequenceSigns.StartsWhenOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsWhenOtherFinished(processB));

			statementsByConstuctor.Add(new ProcessesStatement(processA, processB, SequenceSigns.StartsBeforeOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).StartsBeforeOtherFinished(processB));

			statementsByConstuctor.Add(new ProcessesStatement(processA, processB, SequenceSigns.FinishesAfterOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesAfterOtherFinished(processB));

			statementsByConstuctor.Add(new ProcessesStatement(processA, processB, SequenceSigns.FinishesWhenOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesWhenOtherFinished(processB));

			statementsByConstuctor.Add(new ProcessesStatement(processA, processB, SequenceSigns.FinishesBeforeOtherFinished));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).FinishesBeforeOtherFinished(processB));

			statementsByConstuctor.Add(new ProcessesStatement(processA, processB, SequenceSigns.Causes));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).Causes(processB));

			statementsByConstuctor.Add(new ProcessesStatement(processA, processB, SequenceSigns.IsCausedBy));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).IsCausedBy(processB));

			statementsByConstuctor.Add(new ProcessesStatement(processA, processB, SequenceSigns.SimultaneousWith));
			statementsByBuilder.Add(semanticNetwork.DeclareThat(processA).SimultaneousWith(processB));

			// assert
			for (int s = 0; s < SequenceSigns.All.Count; s++)
			{
				Assert.AreEqual(statementsByConstuctor[s], statementsByBuilder[s]);
			}
		}
	}
}
