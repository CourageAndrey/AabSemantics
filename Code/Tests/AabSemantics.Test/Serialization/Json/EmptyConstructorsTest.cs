using System.Linq;
using System.Reflection;

using NUnit.Framework;

using AabSemantics.Modules.Mathematics.Json;
using AabSemantics.Modules.Boolean.Json;
using AabSemantics.Modules.Classification.Json;
using AabSemantics.Modules.Processes.Json;
using AabSemantics.Serialization.Json;
using AabSemantics.Serialization.Json.Answers;
using AabSemantics.Modules.Set.Json;

namespace AabSemantics.Test.Serialization.Json
{
	[TestFixture]
	public class EmptyConstructorsTest
	{
		[Test]
		public void CheckSemanticNetwork()
		{
			// arrange
			var semanticNetwork = new AabSemantics.Serialization.Json.SemanticNetwork();

			// assert
			Assert.AreEqual(0, semanticNetwork.Name.Values.Count);
			Assert.AreEqual(0, semanticNetwork.Concepts.Count);
			Assert.AreEqual(0, semanticNetwork.Statements.Count);
			Assert.AreEqual(0, semanticNetwork.Modules.Count);
		}

		[Test]
		public void CheckLocalizedString()
		{
			// arrange
			var localizedString = new LocalizedString();

			// assert
			Assert.AreEqual(0, localizedString.Values.Count);
		}

		[Test]
		public void CheckConcept()
		{
			// arrange
			var concept = new Concept();

			// assert
			Assert.IsNull(concept.ID);
			Assert.IsNull(concept.Name);
			Assert.IsNull(concept.Hint);
			Assert.AreEqual(0, concept.Attributes.Count);
		}

		[Test]
		public void CheckStatements()
		{
			var statementsToCheck = new Statement[]
			{
				new ComparisonStatement(),
				new GroupStatement(),
				new HasPartStatement(),
				new HasSignStatement(),
				new IsStatement(),
				new ProcessesStatement(),
				new SignValueStatement(),
			};

			// assert
			foreach (var statement in statementsToCheck)
			{
				var propertiesToCheck = statement.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance).ToList();
				foreach (var property in propertiesToCheck)
				{
					var value = property.GetValue(statement, null);
					Assert.IsNull(value);
				}
			}
		}

		[Test]
		public void CheckQuestions()
		{
			// arrange
			var questionsToCheck = new Question[]
			{
				new CheckStatementQuestion(),
				new EnumerateAncestorsQuestion(),
				new EnumerateDescendantsQuestion(),
				new IsQuestion(),
				new ComparisonQuestion(),
				new ProcessesQuestion(),
				new DescribeSubjectAreaQuestion(),
				new EnumerateContainersQuestion(),
				new EnumeratePartsQuestion(),
				new EnumerateSignsQuestion(),
				new FindSubjectAreaQuestion(),
				new GetCommonQuestion(),
				new GetDifferencesQuestion(),
				new HasSignQuestion(),
				new HasSignsQuestion(),
				new IsPartOfQuestion(),
				new IsSignQuestion(),
				new IsSubjectAreaQuestion(),
				new IsValueQuestion(),
				new SignValueQuestion(),
				new WhatQuestion(),
			};

			// assert
			foreach (var question in questionsToCheck)
			{
				Assert.AreEqual(0, question.Preconditions.Count);
				var propertiesToCheck = question.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance).ToList();
				propertiesToCheck.Remove(propertiesToCheck.First(p => p.Name == nameof(Question.Preconditions)));
				foreach (var property in propertiesToCheck)
				{
					var value = property.GetValue(question, null);
					if (property.PropertyType == typeof(bool))
					{
						Assert.AreEqual(false, value);
					}
					else
					{
						Assert.IsNull(value);
					}
				}
			}
		}

		[Test]
		public void CheckAnswers()
		{
			// arrange
			var emptyAnswer = new Answer();
			var emptyBooleanAnswer = new BooleanAnswer();
			var emptyConceptAnswer = new ConceptAnswer();
			var emptyConceptsAnswer = new ConceptsAnswer();
			var emptyStatementAnswer = new  StatementAnswer();
			var emptyStatementsAnswer = new StatementsAnswer();
			var emptyAnswers = new[]
			{
				emptyAnswer,
				emptyBooleanAnswer,
				emptyConceptAnswer,
				emptyConceptsAnswer,
				emptyStatementAnswer,
				emptyStatementsAnswer,
			};

			// assert
			Assert.IsFalse(emptyBooleanAnswer.Result);
			Assert.IsNull(emptyConceptAnswer.Concept);
			Assert.AreEqual(0, emptyConceptsAnswer.Concepts.Count);
			Assert.IsNull(emptyStatementAnswer.Statement);
			Assert.AreEqual(0, emptyStatementsAnswer.Statements.Count);
			foreach (var answer in emptyAnswers)
			{
				Assert.AreEqual(true, answer.IsEmpty);
				Assert.IsTrue(string.IsNullOrEmpty(answer.Description));
				Assert.AreEqual(0, answer.Explanation.Count);
			}
		}
	}
}
