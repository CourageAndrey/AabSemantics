﻿using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Contexts;
using AabSemantics.Localization;
using AabSemantics.Modules.Classification.Questions;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;
using AabSemantics.Test.Sample;

namespace AabSemantics.Tests.Questions
{
	[TestFixture]
	public class StatementQuestionProcessorTest
	{
		[Test]
		public void GivenCustomQuestionProcessor_WhenProcess_ThenSucceed()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default);
			var question = new IsQuestion(1.CreateConcept(), 2.CreateConcept());
			var questionContext = new QuestionProcessingContext<IsQuestion>(semanticNetwork.Context, question);
			var processor = new TestQuestionProcessor(questionContext, null);

			// act & assert
			Assert.AreSame(questionContext, processor.GetContext());
			Assert.IsNotNull(processor.GetStatements());
			Assert.IsNotNull(processor.GetChildAnswers());
			Assert.IsNotNull(processor.GetAdditionalTransitives());
			Assert.IsNotNull(processor.IsItNecessaryToProcessTransitives(Array.Empty<IsStatement>()));
			Assert.IsNotNull(processor.EnumerateTransitiveQuestions(questionContext));
			Assert.DoesNotThrow(() => processor.IsItNecessaryToAggregateTransitivesToStatements());
		}

		[Test]
		public void GivenTransitiveStatements_WhenProcess_ThenSucceed()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default);
			var testData = semanticNetwork.CreateSetTestData();
			semanticNetwork.CreateMathematicsTestData();
			semanticNetwork.CreateProcessesTestData();

			var thomasTheTrain = "TtT".CreateConcept();
			semanticNetwork.Concepts.Add(thomasTheTrain);
			var testIs = semanticNetwork.DeclareThat(thomasTheTrain).IsDescendantOf(testData.Vehicle_SteamLocomotive);

			var question = new IsQuestion(thomasTheTrain, testData.Base_Vehicle);
			var questionContext = new QuestionProcessingContext<IsQuestion>(semanticNetwork.Context, question);

			var additionalStatement = semanticNetwork.Statements.First();

			// act
			var questionProcessor = new TestQuestionProcessor(questionContext, additionalStatement);

			questionProcessor
				.WithTransitives(
					rr => rr.Count == 0,
					q => q.Child,
					newSubject => new IsQuestion(newSubject, question.Parent),
					true) // this line differs from regular IsQuestion processor
				.Where(s => s.Parent == question.Parent && s.Child == question.Child);

			var answer = questionProcessor
				.SelectBooleanIncludingChildren(
					statements => statements.Count > 0,
					language => string.Empty,
					language => string.Empty,
					new Dictionary<String, IKnowledge>());

			// assert
			Assert.IsTrue(answer.Explanation.Statements.Count >= 3);
			Assert.IsTrue(answer.Explanation.Statements.Any(s => (s as IsStatement)?.Parent == testData.Base_Vehicle && (s as IsStatement)?.Child == testData.Vehicle_SteamLocomotive));
			Assert.IsTrue(answer.Explanation.Statements.Contains(testIs));
			Assert.IsTrue(answer.Explanation.Statements.Contains(additionalStatement));
		}

		public class TestQuestionProcessor : StatementQuestionProcessor<IsQuestion, IsStatement>
		{
			private readonly IStatement _additionalStatement;

			public TestQuestionProcessor(
				IQuestionProcessingContext<IsQuestion> context,
				IStatement additionalStatement)
				: base(context)
			{
				_additionalStatement = additionalStatement;
			}

			protected override void DoAggregateTransitivesToStatements()
			{
				if (_additionalStatement != null)
				{
					var childAnswer = ChildAnswers.First();
					childAnswer.Answer.Explanation.Statements.Add(_additionalStatement);
				}

				base.DoAggregateTransitivesToStatements();
			}

			public IQuestionProcessingContext<IsQuestion> GetContext()
			{
				return Context;
			}

			public ICollection<IsStatement> GetStatements()
			{
				return Statements;
			}

			public ICollection<ChildAnswer> GetChildAnswers()
			{
				return ChildAnswers;
			}

			public ICollection<IStatement> GetAdditionalTransitives()
			{
				return AdditionalTransitives;
			}

			public Boolean IsItNecessaryToProcessTransitives(ICollection<IsStatement> statements)
			{
				return NeedToProcessTransitives(statements);
			}

			public IEnumerable<NestedQuestion> EnumerateTransitiveQuestions(IQuestionProcessingContext<IsQuestion> context)
			{
				return GetTransitiveQuestions(context);
			}

			public Boolean IsItNecessaryToAggregateTransitivesToStatements()
			{
				return NeedToAggregateTransitivesToStatements;
			}
		}
	}
}
