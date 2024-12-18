using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Contexts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Questions;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Questions;
using AabSemantics.Statements;
using AabSemantics.TestCore;

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
			var question = new IsQuestion(1.CreateConceptByObject(), 2.CreateConceptByObject());
			var questionContext = new QuestionProcessingContext<IsQuestion>(semanticNetwork.Context, question);
			var processor = new TestQuestionProcessor(questionContext, null);

			// act & assert
			Assert.That(processor.GetContext(), Is.SameAs(questionContext));
			Assert.That(processor.GetStatements(), Is.Not.Null);
			Assert.That(processor.GetChildAnswers(), Is.Not.Null);
			Assert.That(processor.GetAdditionalTransitives(), Is.Not.Null);
			Assert.That(processor.IsItNecessaryToProcessTransitives(Array.Empty<IsStatement>()), Is.Not.Null);
			Assert.That(processor.EnumerateTransitiveQuestions(questionContext), Is.Not.Null);
			Assert.DoesNotThrow(() => processor.IsItNecessaryToAggregateTransitivesToStatements());
		}

		[Test]
		public void GivenTransitiveStatements_WhenProcess_ThenSucceed()
		{
			// arrange
			var semanticNetwork = new SemanticNetwork(Language.Default);
			var testData = semanticNetwork
				.WithModule<BooleanModule>()
				.WithModule<ClassificationModule>();

			IConcept vehicle, steamLocomotive, thomasTheTrain;
			semanticNetwork.Concepts.Add(vehicle = "vehicle".CreateConceptByName());
			semanticNetwork.Concepts.Add(steamLocomotive = "steamLocomotive".CreateConceptByName());
			semanticNetwork.Concepts.Add(thomasTheTrain = "thomasTheTrain".CreateConceptByName());
			semanticNetwork.DeclareThat(steamLocomotive).IsDescendantOf(vehicle);
			var testIs = semanticNetwork.DeclareThat(thomasTheTrain).IsDescendantOf(steamLocomotive);

			var question = new IsQuestion(thomasTheTrain, vehicle);
			var questionContext = new QuestionProcessingContext<IsQuestion>(semanticNetwork.Context, question);

			var additionalStatement = new TestStatement();
			semanticNetwork.Statements.Add(additionalStatement);

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
			Assert.That(answer.Explanation.Statements.Count >= 3, Is.True);
			Assert.That(answer.Explanation.Statements.Any(s => (s as IsStatement)?.Parent == vehicle && (s as IsStatement)?.Child == steamLocomotive), Is.True);
			Assert.That(answer.Explanation.Statements.Contains(testIs), Is.True);
			Assert.That(answer.Explanation.Statements.Contains(additionalStatement), Is.True);
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
