using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Core;
using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Questions;
using Inventor.Core.Statements;

namespace Inventor.Test.Questions
{
	[TestFixture]
	public class ComparisonQuestionTest
	{
		[Test]
		public void CheckUncomparableValues()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);

			// act
			var question = new ComparisonQuestion(ComparisonSigns.IsNotEqualTo, SequenceSigns.Causes);
			var answer = question.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			Assert.IsTrue(answer.IsEmpty);
			Assert.AreEqual(0, answer.Explanation.Statements.Count);
		}

		[Test]
		public void CheckEquality()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);

			// act
			var question1 = new ComparisonQuestion(knowledgeBase.Number0, knowledgeBase.NumberZero);
			var answer1 = question1.Ask(knowledgeBase.KnowledgeBase.Context);

			var question2 = new ComparisonQuestion(knowledgeBase.NumberZero, knowledgeBase.Number0);
			var answer2 = question2.Ask(knowledgeBase.KnowledgeBase.Context);

			// assert
			var explanation1 = (ComparisonStatement) answer1.Explanation.Statements.Single();
			var explanation2 = (ComparisonStatement) answer2.Explanation.Statements.Single();
			Assert.AreSame(explanation1, explanation2);
			Assert.IsTrue(explanation1.GetChildConcepts().Contains(knowledgeBase.Number0));
			Assert.IsTrue(explanation1.GetChildConcepts().Contains(knowledgeBase.NumberZero));
			Assert.IsTrue(explanation1.GetChildConcepts().Contains(ComparisonSigns.IsEqualTo));

			var statement1 = (ComparisonStatement) ((StatementAnswer) answer1).Result;
			Assert.AreSame(knowledgeBase.Number0, statement1.LeftValue);
			Assert.AreSame(knowledgeBase.NumberZero, statement1.RightValue);
			Assert.AreSame(ComparisonSigns.IsEqualTo, statement1.ComparisonSign);

			var statement2 = (ComparisonStatement) ((StatementAnswer) answer2).Result;
			Assert.AreSame(knowledgeBase.NumberZero, statement2.LeftValue);
			Assert.AreSame(knowledgeBase.Number0, statement2.RightValue);
			Assert.AreSame(ComparisonSigns.IsEqualTo, statement2.ComparisonSign);
		}

		[Test]
		public void CheckGreaterAndLessConditions()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);

			var orderedNumbersWith0 = new List<IConcept>
			{
				knowledgeBase.Number0,
				knowledgeBase.Number1,
				knowledgeBase.Number2,
				knowledgeBase.Number3,
				knowledgeBase.Number4,
			};

			var orderedNumbersWithZero = new List<IConcept>
			{
				knowledgeBase.NumberZero,
				knowledgeBase.Number1,
				knowledgeBase.Number2,
				knowledgeBase.Number3,
				knowledgeBase.Number4,
			};

			foreach (var orderedNumbers in new[] { orderedNumbersWith0, orderedNumbersWithZero })
			{
				for (int l = 0; l < orderedNumbers.Count; l++)
				{
					for (int r = 0; r < orderedNumbers.Count; r++)
					{
						if (l != r) // because "A=A" automatic precondition is not defined
						{
							// act
							var question = new ComparisonQuestion(orderedNumbers[l], orderedNumbers[r]);
							var answer = question.Ask(knowledgeBase.KnowledgeBase.Context);

							// assert
							var explanation = answer.Explanation.Statements;
							int expectedExplanationStatementsCount = Math.Abs(l - r);
							if (orderedNumbers == orderedNumbersWithZero && (l == 0 || r == 0))
							{
								expectedExplanationStatementsCount++;
							}
							Assert.AreEqual(expectedExplanationStatementsCount, explanation.Count);
							Assert.AreEqual(explanation.Count, explanation.OfType<ComparisonStatement>().Count());

							var statement = (ComparisonStatement)((StatementAnswer)answer).Result;
							Assert.AreSame(orderedNumbers[l], statement.LeftValue);
							Assert.AreSame(orderedNumbers[r], statement.RightValue);
							Assert.AreSame(l > r ? ComparisonSigns.IsGreaterThan : ComparisonSigns.IsLessThan, statement.ComparisonSign);
						}
					}
				}
			}
		}

		[Test]
		public void CheckConditionsWithLeAndGe()
		{
			// arrange
			var language = Language.Default;
			var knowledgeBase = new TestKnowledgeBase(language);

			var comparisons = new[]
			{
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number1or2, knowledgeBase.Number0, ComparisonSigns.IsGreaterThan),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number1or2, knowledgeBase.Number1, ComparisonSigns.IsGreaterThanOrEqualTo),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number1or2, knowledgeBase.Number2, ComparisonSigns.IsLessThanOrEqualTo),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number1or2, knowledgeBase.Number3, ComparisonSigns.IsLessThan),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number1or2, knowledgeBase.Number4, ComparisonSigns.IsLessThan),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number1or2, knowledgeBase.Number2or3, ComparisonSigns.IsLessThanOrEqualTo),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number1or2, knowledgeBase.Number3or4, ComparisonSigns.IsLessThan),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number2or3, knowledgeBase.Number0, ComparisonSigns.IsGreaterThan),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number2or3, knowledgeBase.Number1, ComparisonSigns.IsGreaterThan),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number2or3, knowledgeBase.Number2, ComparisonSigns.IsGreaterThanOrEqualTo),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number2or3, knowledgeBase.Number3, ComparisonSigns.IsLessThanOrEqualTo),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number2or3, knowledgeBase.Number4, ComparisonSigns.IsLessThan),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number2or3, knowledgeBase.Number1or2, ComparisonSigns.IsGreaterThanOrEqualTo),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number2or3, knowledgeBase.Number3or4, ComparisonSigns.IsLessThanOrEqualTo),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number3or4, knowledgeBase.Number0, ComparisonSigns.IsGreaterThan),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number3or4, knowledgeBase.Number1, ComparisonSigns.IsGreaterThan),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number3or4, knowledgeBase.Number2, ComparisonSigns.IsGreaterThan),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number3or4, knowledgeBase.Number3, ComparisonSigns.IsGreaterThanOrEqualTo),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number3or4, knowledgeBase.Number4, ComparisonSigns.IsLessThanOrEqualTo),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number3or4, knowledgeBase.Number1or2, ComparisonSigns.IsGreaterThan),
				new Tuple<IConcept, IConcept, IConcept>(knowledgeBase.Number3or4, knowledgeBase.Number2or3, ComparisonSigns.IsGreaterThanOrEqualTo),
			};

			foreach (var comparison in comparisons)
			{
				// act
				var question1 = new ComparisonQuestion(comparison.Item1, comparison.Item2);
				var answer1 = question1.Ask(knowledgeBase.KnowledgeBase.Context);
				var statement1 = (ComparisonStatement) ((StatementAnswer) answer1).Result;

				var question2 = new ComparisonQuestion(comparison.Item2, comparison.Item1);
				var answer2 = question2.Ask(knowledgeBase.KnowledgeBase.Context);
				var statement2 = (ComparisonStatement) ((StatementAnswer) answer2).Result;

				// assert
				Assert.AreSame(comparison.Item1, statement1.LeftValue);
				Assert.AreSame(comparison.Item2, statement1.RightValue);
				Assert.AreSame(comparison.Item3, statement1.ComparisonSign);

				Assert.AreSame(comparison.Item2, statement2.LeftValue);
				Assert.AreSame(comparison.Item1, statement2.RightValue);
				Assert.AreSame(ComparisonSigns.Revert(comparison.Item3), statement2.ComparisonSign);
			}
		}
	}
}
