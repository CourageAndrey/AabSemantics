using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Answers;
using Inventor.Semantics.Text.Containers;
using Inventor.Semantics.Text.Primitives;

namespace Inventor.Semantics.Questions
{
	public class StatementQuestionProcessor<QuestionT, StatementT>
		where QuestionT : IQuestion
		where StatementT : IStatement
	{
		#region Properties

		protected IQuestionProcessingContext<QuestionT> Context
		{ get; }

		protected ICollection<StatementT> Statements
		{ get; private set; }

		protected ICollection<ChildAnswer> ChildAnswers
		{ get; private set; }

		protected ICollection<IStatement> AdditionalTransitives
		{ get; private set; }

		protected Func<ICollection<StatementT>, Boolean> NeedToProcessTransitives
		{ get; private set; }

		protected Func<IQuestionProcessingContext<QuestionT>, IEnumerable<NestedQuestion>> GetTransitiveQuestions
		{ get; private set; }

		protected Boolean NeedToAggregateTransitivesToStatements
		{ get; private set; }

		#endregion

		public StatementQuestionProcessor(IQuestionProcessingContext context)
		{
			Context = (IQuestionProcessingContext<QuestionT>) context;
			Statements = Array.Empty<StatementT>();
			ChildAnswers = Array.Empty<ChildAnswer>();
			AdditionalTransitives = Array.Empty<IStatement>();
			NeedToProcessTransitives = statements => false;
			GetTransitiveQuestions = c => Array.Empty<NestedQuestion>();
			NeedToAggregateTransitivesToStatements = false;
		}

		public StatementQuestionProcessor<QuestionT, StatementT> Where(Func<StatementT, Boolean> match)
		{
			Statements = Context.SemanticNetwork.Statements
				.Enumerate<StatementT>(Context.ActiveContexts)
				.Where(match)
				.ToList();

			return this;
		}

		public StatementQuestionProcessor<QuestionT, StatementT> WithTransitives(
			Func<ICollection<StatementT>, Boolean> needToProcessTransitives,
			Func<IQuestionProcessingContext<QuestionT>, IEnumerable<NestedQuestion>> getTransitiveQuestions,
			Boolean needToAggregateTransitivesToStatements = false)
		{
			NeedToProcessTransitives = needToProcessTransitives;
			GetTransitiveQuestions = getTransitiveQuestions;
			NeedToAggregateTransitivesToStatements = needToAggregateTransitivesToStatements;

			return this;
		}

		public StatementQuestionProcessor<QuestionT, StatementT> WithTransitives(
			Func<ICollection<StatementT>, Boolean> needToProcessTransitives,
			Func<QuestionT, IConcept> getQuestionSubject,
			Func<IConcept, QuestionT> createQuestionForSubject,
			Boolean needToAggregateTransitivesToStatements = false)
		{
			return WithTransitives(
				needToProcessTransitives,
				context => GetNestedQuestions(getQuestionSubject, createQuestionForSubject),
				needToAggregateTransitivesToStatements);
		}

		public IAnswer SelectCustom(Func<IQuestionProcessingContext<QuestionT>, ICollection<StatementT>, ICollection<ChildAnswer>, IAnswer> formatter)
		{
			ProcessChildrenIfNeed();

			var answer = formatter(Context, Statements, ChildAnswers);

			answer.Explanation.Expand(AdditionalTransitives);

			return answer;
		}

		public IAnswer SelectAllConcepts(
			Func<StatementT, IConcept> resultConceptSelector,
			Func<QuestionT, IConcept> titleConceptSelector,
			String titleConceptCaption,
			Func<ILanguage, String> answerFormat,
			Func<IEnumerable<IConcept>, IEnumerable<IConcept>> conceptsFilter = null)
		{
			ProcessChildrenIfNeed();

			if (Statements.Any())
			{
				if (conceptsFilter == null)
				{
					conceptsFilter = concepts => concepts;
				}

				var resultConcepts = conceptsFilter(Statements.Select(resultConceptSelector)).ToList();

				var format = new UnstructuredContainer(new FormattedText(
					language => answerFormat(Context.Language),
					new Dictionary<String, IKnowledge>
					{
						{ titleConceptCaption, titleConceptSelector(Context.Question) },
					})).AppendBulletsList(resultConcepts.Enumerate());

				var answer = new ConceptsAnswer(
					resultConcepts,
					format,
					new Explanation(Statements.OfType<IStatement>()));

				answer.Explanation.Expand(AdditionalTransitives);

				return answer;
			}
			else
			{
				return Answer.CreateUnknown();
			}
		}

		public IAnswer SelectFirstConcept(
			Func<StatementT, IConcept> resultConceptSelector,
			Func<ILanguage, String> answerFormat,
			Func<StatementT, IDictionary<String, IKnowledge>> getParameters)
		{
			ProcessChildrenIfNeed();

			IAnswer answer = null;

			var statement = Statements.FirstOrDefault();
			if (statement != null)
			{
				answer = new ConceptAnswer(
					resultConceptSelector(statement),
					new FormattedText(
						answerFormat,
						getParameters(statement)),
					new Explanation(Statements.OfType<IStatement>()));

				answer.Explanation.Expand(AdditionalTransitives);
			}

			if (answer == null)
			{
				var childAnswer = ChildAnswers.FirstOrDefault();
				if (childAnswer != null)
				{
					childAnswer.Answer.Explanation.Expand(childAnswer.TransitiveStatements);
					answer = childAnswer.Answer;
				}
			}

			return answer ?? Answer.CreateUnknown();
		}

		public BooleanAnswer SelectBoolean(
			Func<ICollection<StatementT>, Boolean> valueGetter,
			Func<ILanguage, String> trueFormat,
			Func<ILanguage, String> falseFormat,
			IDictionary<String, IKnowledge> parameters)
		{
			ProcessChildrenIfNeed();

			Boolean value = valueGetter(Statements);

			var answer = new BooleanAnswer(
				value,
				new FormattedText(
					value ? trueFormat : falseFormat,
					parameters),
				new Explanation(Statements.OfType<IStatement>()));

			answer.Explanation.Expand(AdditionalTransitives);

			return answer;
		}

		public BooleanAnswer SelectBooleanIncludingChildren(
			Func<ICollection<StatementT>, Boolean> valueGetter,
			Func<ILanguage, String> trueFormat,
			Func<ILanguage, String> falseFormat,
			IDictionary<String, IKnowledge> parameters)
		{
			ProcessChildrenIfNeed();

			Boolean result = false;
			var explanation = new List<IStatement>(Statements.OfType<IStatement>());

			if (Statements.Count > 0)
			{
				result = true;
			}

			foreach (var childAnswer in ChildAnswers)
			{
				if (((BooleanAnswer) childAnswer.Answer).Result)
				{
					result = true;
					explanation.AddRange(childAnswer.Answer.Explanation.Statements);
					explanation.AddRange(childAnswer.TransitiveStatements);
				}
			}

			var answer = new BooleanAnswer(
				result,
				new FormattedText(
					result ? trueFormat : falseFormat,
					parameters),
				new Explanation(explanation));

			answer.Explanation.Expand(AdditionalTransitives);

			return answer;
		}

		protected virtual void ProcessChildrenIfNeed()
		{
			if (NeedToProcessTransitives(Statements))
			{
				ChildAnswers = new List<ChildAnswer>();
				foreach (var nested in GetTransitiveQuestions(Context))
				{
					var answer = nested.Question.Ask(Context);
					if (!answer.IsEmpty)
					{
						ChildAnswers.Add(new ChildAnswer(nested.Question, answer, nested.TransitiveStatements));
					}
				}

				if (NeedToAggregateTransitivesToStatements)
				{
					DoAggregateTransitivesToStatements();
				}
			}
			else
			{
				ChildAnswers = Array.Empty<ChildAnswer>();
			}
		}

		protected virtual IEnumerable<NestedQuestion> GetNestedQuestions(
			Func<QuestionT, IConcept> getQuestionSubject,
			Func<IConcept, QuestionT> createQuestionForSubject)
		{
			var alreadyViewedConcepts = new HashSet<IConcept>(Context.ActiveContexts
				.OfType<IQuestionProcessingContext<QuestionT>>()
				.Select(questionContext => getQuestionSubject(questionContext.Question)));

			var question = Context.Question;
			var subject = getQuestionSubject(question);
			var transitiveStatements = Context.SemanticNetwork.Statements
				.Enumerate<Modules.Classification.Statements.IsStatement>(Context.ActiveContexts)
				.Where(isStatement => isStatement.Child == subject);

			foreach (var transitiveStatement in transitiveStatements)
			{
				var parent = transitiveStatement.Parent;
				if (!alreadyViewedConcepts.Contains(parent))
				{
					yield return new NestedQuestion(createQuestionForSubject(parent), new IStatement[] { transitiveStatement });
				}
			}
		}

		protected virtual void DoAggregateTransitivesToStatements()
		{
			var additionalTransitives = new List<IStatement>();
			foreach (var answer in ChildAnswers)
			{
				foreach (var statement in answer.Answer.Explanation.Statements)
				{
					if (statement is StatementT)
					{
						Statements.Add((StatementT) statement);
					}
					else
					{
						additionalTransitives.Add(statement);
					}
				}

				if (!answer.Answer.IsEmpty)
				{
					additionalTransitives.AddRange(answer.TransitiveStatements);
				}
			}

			if (additionalTransitives.Count > 0)
			{
				AdditionalTransitives = additionalTransitives;
			}
		}
	}
}