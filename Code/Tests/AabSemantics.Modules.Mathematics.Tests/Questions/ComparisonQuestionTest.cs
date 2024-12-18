using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Answers;
using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Concepts;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Mathematics.Localization;
using AabSemantics.Modules.Mathematics.Questions;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.Questions;

namespace AabSemantics.Modules.Mathematics.Tests.Questions
{
	[TestFixture]
	public class ComparisonQuestionTest
	{
		[Test]
		public void GivenNullArguments_WhenTryToCreateQuestion_ThenFail()
		{
			// arrange
			IConcept concept = "test".CreateConceptByName();

			// act && assert
			Assert.Throws<ArgumentNullException>(() => new ComparisonQuestion(null, concept));
			Assert.Throws<ArgumentNullException>(() => new ComparisonQuestion(concept, null));
		}

		[Test]
		public void GivenHowCompared_WhenBeingAsked_ThenBuildAndAskQuestion()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateMathematicsTestData();

			// act
			var questionRegular = new ComparisonQuestion(semanticNetwork.Number0, semanticNetwork.Number2);
			var answerRegular = (StatementAnswer) questionRegular.Ask(semanticNetwork.SemanticNetwork.Context);

			var answerBuilder = (StatementAnswer) semanticNetwork.SemanticNetwork.Ask().HowCompared(semanticNetwork.Number0, semanticNetwork.Number2);

			// assert
			Assert.That(answerBuilder.Result, Is.EqualTo(answerRegular.Result));
			Assert.That(answerRegular.Explanation.Statements.SequenceEqual(answerBuilder.Explanation.Statements), Is.True);
		}

		[Test]
		public void GivenNoInformation_WhenBeingAsked_ThenReturnEmpty()
		{
			// arrange
			var textRender = TextRenders.PlainString;

			var language = Language.Default;
			language.Extensions.Add(LanguageBooleanModule.CreateDefault());
			language.Extensions.Add(LanguageClassificationModule.CreateDefault());
			language.Extensions.Add(LanguageMathematicsModule.CreateDefault());

			var semanticNetwork = new SemanticNetwork(language);

			var question = new ComparisonQuestion(1.CreateConceptByObject(), 2.CreateConceptByObject());

			// act
			var answer = question.Ask(semanticNetwork.Context);
			var description = textRender.RenderText(answer.Description, language).ToString();

			// assert
			Assert.That(answer.IsEmpty, Is.True);
			Assert.That(description.Contains(language.Questions.Answers.Unknown), Is.True);
			Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenIncomparableValues_WhenBeingAsked_ThenReturnEmpty()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateMathematicsTestData();

			// act
			var answer = semanticNetwork.SemanticNetwork.Ask().HowCompared(ComparisonSigns.IsNotEqualTo, LogicalValues.False);

			// assert
			Assert.That(answer.IsEmpty, Is.True);
			Assert.That(answer.Explanation.Statements.Count, Is.EqualTo(0));
		}

		[Test]
		public void GivenEqualConditions_WhenBeingAsked_ThenReturnResult()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateMathematicsTestData();

			// act
			var answer1 = semanticNetwork.SemanticNetwork.Ask().HowCompared(semanticNetwork.Number0, semanticNetwork.NumberZero);

			var answer2 = semanticNetwork.SemanticNetwork.Ask().HowCompared(semanticNetwork.NumberZero, semanticNetwork.Number0);

			// assert
			var explanation1 = (ComparisonStatement) answer1.Explanation.Statements.Single();
			var explanation2 = (ComparisonStatement) answer2.Explanation.Statements.Single();
			Assert.That(explanation2, Is.SameAs(explanation1));
			Assert.That(explanation1.GetChildConcepts().Contains(semanticNetwork.Number0), Is.True);
			Assert.That(explanation1.GetChildConcepts().Contains(semanticNetwork.NumberZero), Is.True);
			Assert.That(explanation1.GetChildConcepts().Contains(ComparisonSigns.IsEqualTo), Is.True);

			var statement1 = (ComparisonStatement) ((StatementAnswer) answer1).Result;
			Assert.That(statement1.LeftValue, Is.SameAs(semanticNetwork.Number0));
			Assert.That(statement1.RightValue, Is.SameAs(semanticNetwork.NumberZero));
			Assert.That(statement1.ComparisonSign, Is.SameAs(ComparisonSigns.IsEqualTo));

			var statement2 = (ComparisonStatement) ((StatementAnswer) answer2).Result;
			Assert.That(statement2.LeftValue, Is.SameAs(semanticNetwork.NumberZero));
			Assert.That(statement2.RightValue, Is.SameAs(semanticNetwork.Number0));
			Assert.That(statement2.ComparisonSign, Is.SameAs(ComparisonSigns.IsEqualTo));
		}

		[Test]
		public void GivenGreaterAndLessConditions_WhenBeingAsked_ThenReturnResult()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateMathematicsTestData();

			var orderedNumbersWith0 = new List<IConcept>
			{
				semanticNetwork.Number0,
				semanticNetwork.Number1,
				semanticNetwork.Number2,
				semanticNetwork.Number3,
				semanticNetwork.Number4,
			};

			var orderedNumbersWithZero = new List<IConcept>
			{
				semanticNetwork.NumberZero,
				semanticNetwork.Number1,
				semanticNetwork.Number2,
				semanticNetwork.Number3,
				semanticNetwork.Number4,
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
							var answer = semanticNetwork.SemanticNetwork.Ask().HowCompared(orderedNumbers[l], orderedNumbers[r]);

							// assert
							var explanation = answer.Explanation.Statements;
							int expectedExplanationStatementsCount = Math.Abs(l - r);
							if (orderedNumbers == orderedNumbersWithZero && (l == 0 || r == 0))
							{
								expectedExplanationStatementsCount++;
							}
							Assert.That(explanation.Count, Is.EqualTo(expectedExplanationStatementsCount));
							Assert.That(explanation.OfType<ComparisonStatement>().Count(), Is.EqualTo(explanation.Count));

							var statement = (ComparisonStatement)((StatementAnswer)answer).Result;
							Assert.That(statement.LeftValue, Is.SameAs(orderedNumbers[l]));
							Assert.That(statement.RightValue, Is.SameAs(orderedNumbers[r]));
							Assert.That(statement.ComparisonSign, Is.SameAs(l > r ? ComparisonSigns.IsGreaterThan : ComparisonSigns.IsLessThan));
						}
					}
				}
			}
		}

		[Test]
		public void GivenConditionsWithLeAndGe_WhenBeingAsked_ThenReturnResult()
		{
			// arrange
			var language = Language.Default;
			var semanticNetwork = new SemanticNetwork(language).CreateMathematicsTestData();

			var comparisons = new[]
			{
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number1or2, semanticNetwork.Number0, ComparisonSigns.IsGreaterThan),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number1or2, semanticNetwork.Number1, ComparisonSigns.IsGreaterThanOrEqualTo),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number1or2, semanticNetwork.Number2, ComparisonSigns.IsLessThanOrEqualTo),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number1or2, semanticNetwork.Number3, ComparisonSigns.IsLessThan),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number1or2, semanticNetwork.Number4, ComparisonSigns.IsLessThan),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number1or2, semanticNetwork.Number2or3, ComparisonSigns.IsLessThanOrEqualTo),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number1or2, semanticNetwork.Number3or4, ComparisonSigns.IsLessThan),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number2or3, semanticNetwork.Number0, ComparisonSigns.IsGreaterThan),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number2or3, semanticNetwork.Number1, ComparisonSigns.IsGreaterThan),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number2or3, semanticNetwork.Number2, ComparisonSigns.IsGreaterThanOrEqualTo),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number2or3, semanticNetwork.Number3, ComparisonSigns.IsLessThanOrEqualTo),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number2or3, semanticNetwork.Number4, ComparisonSigns.IsLessThan),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number2or3, semanticNetwork.Number1or2, ComparisonSigns.IsGreaterThanOrEqualTo),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number2or3, semanticNetwork.Number3or4, ComparisonSigns.IsLessThanOrEqualTo),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number3or4, semanticNetwork.Number0, ComparisonSigns.IsGreaterThan),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number3or4, semanticNetwork.Number1, ComparisonSigns.IsGreaterThan),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number3or4, semanticNetwork.Number2, ComparisonSigns.IsGreaterThan),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number3or4, semanticNetwork.Number3, ComparisonSigns.IsGreaterThanOrEqualTo),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number3or4, semanticNetwork.Number4, ComparisonSigns.IsLessThanOrEqualTo),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number3or4, semanticNetwork.Number1or2, ComparisonSigns.IsGreaterThan),
				new Tuple<IConcept, IConcept, IConcept>(semanticNetwork.Number3or4, semanticNetwork.Number2or3, ComparisonSigns.IsGreaterThanOrEqualTo),
			};

			foreach (var comparison in comparisons)
			{
				// act
				var answer1 = semanticNetwork.SemanticNetwork.Ask().HowCompared(comparison.Item1, comparison.Item2);
				var statement1 = (ComparisonStatement) ((StatementAnswer) answer1).Result;

				var answer2 = semanticNetwork.SemanticNetwork.Ask().HowCompared(comparison.Item2, comparison.Item1);
				var statement2 = (ComparisonStatement) ((StatementAnswer) answer2).Result;

				// assert
				Assert.That(statement1.LeftValue, Is.SameAs(comparison.Item1));
				Assert.That(statement1.RightValue, Is.SameAs(comparison.Item2));
				Assert.That(statement1.ComparisonSign, Is.SameAs(comparison.Item3));

				Assert.That(statement2.LeftValue, Is.SameAs(comparison.Item2));
				Assert.That(statement2.RightValue, Is.SameAs(comparison.Item1));
				Assert.That(statement2.ComparisonSign, Is.SameAs(ComparisonSigns.Revert(comparison.Item3)));
			}
		}
	}
}
