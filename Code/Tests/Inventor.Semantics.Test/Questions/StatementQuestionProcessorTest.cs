using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Inventor.Semantics.Concepts;
using Inventor.Semantics.Contexts;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Modules.Classification.Questions;
using Inventor.Semantics.Modules.Classification.Statements;
using Inventor.Semantics.Questions;
using Inventor.Semantics.Statements;
using Inventor.Semantics.Test.Sample;

namespace Inventor.Semantics.Test.Questions
{
	[TestFixture]
	public class StatementQuestionProcessorTest
	{
		[Test]
		public void StatementQuestionProcessorWorksByDefault()
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
			Assert.IsNotNull(processor.IsItNeccessaryToProcessTransitives(Array.Empty<IsStatement>()));
			Assert.IsNotNull(processor.EnumerateTransitiveQuestions(questionContext));
			Assert.DoesNotThrow(() => processor.IsItNeccessaryToAggregateTransitivesToStatements());
		}

		[Test]
		public void CheckAdditionalTransitives()
		{
			// arrange
			var testData = new TestSemanticNetwork(Language.Default);
			var semanticNetwork = testData.SemanticNetwork;

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

			public Boolean IsItNeccessaryToProcessTransitives(ICollection<IsStatement> statements)
			{
				return NeedToProcessTransitives(statements);
			}

			public IEnumerable<NestedQuestion> EnumerateTransitiveQuestions(IQuestionProcessingContext<IsQuestion> context)
			{
				return GetTransitiveQuestions(context);
			}

			public Boolean IsItNeccessaryToAggregateTransitivesToStatements()
			{
				return NeedToAggregateTransitivesToStatements;
			}
		}
	}
}
