using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Answers;
using AabSemantics.Text.Containers;
using AabSemantics.Text.Primitives;
using AabSemantics.Utils;

namespace AabSemantics.Questions
{
	public class StatementQuestionProcessor<QuestionT, StatementT>
		where QuestionT : IQuestion
		where StatementT : class, IStatement
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

		protected Func<ICollection<StatementT>, Task<Boolean>> NeedToProcessTransitives
		{ get; private set; }

		protected Func<IQuestionProcessingContext<QuestionT>, IEnumerable<NestedQuestion>> GetTransitiveQuestions
		{ get; private set; }

		protected Boolean NeedToAggregateTransitivesToStatements
		{ get; private set; }

		private Task<List<StatementT>> _whereTask;

		#endregion

		public StatementQuestionProcessor(IQuestionProcessingContext context)
		{
			Context = (IQuestionProcessingContext<QuestionT>) context;
			Statements = Array.Empty<StatementT>();
			ChildAnswers = Array.Empty<ChildAnswer>();
			AdditionalTransitives = Array.Empty<IStatement>();
			NeedToProcessTransitives = statements => Task.FromResult(false);
			GetTransitiveQuestions = c => Array.Empty<NestedQuestion>();
			NeedToAggregateTransitivesToStatements = false;
		}

		public StatementQuestionProcessor<QuestionT, StatementT> Where(Func<StatementT, Boolean> match)
		{
			_whereTask = Context.SemanticNetwork.Statements
				.Enumerate<StatementT>(Context.ActiveContexts)
				.Where(match)
				.ToListAsync();

			return this;
		}

		public StatementQuestionProcessor<QuestionT, StatementT> WithTransitives(
			Func<ICollection<StatementT>, Task<Boolean>> needToProcessTransitives,
			Func<IQuestionProcessingContext<QuestionT>, IEnumerable<NestedQuestion>> getTransitiveQuestions,
			Boolean needToAggregateTransitivesToStatements = false)
		{
			NeedToProcessTransitives = needToProcessTransitives;
			GetTransitiveQuestions = getTransitiveQuestions;
			NeedToAggregateTransitivesToStatements = needToAggregateTransitivesToStatements;

			return this;
		}

		public StatementQuestionProcessor<QuestionT, StatementT> WithTransitives(
			Func<ICollection<StatementT>, Task<Boolean>> needToProcessTransitives,
			Func<QuestionT, IConcept> getQuestionSubject,
			Func<IConcept, QuestionT> createQuestionForSubject,
			Boolean needToAggregateTransitivesToStatements = false)
		{
			return WithTransitives(
				needToProcessTransitives,
				context => GetNestedQuestions(getQuestionSubject, createQuestionForSubject),
				needToAggregateTransitivesToStatements);
		}

		public async Task<IAnswer> SelectCustomAsync(Func<IQuestionProcessingContext<QuestionT>, ICollection<StatementT>, ICollection<ChildAnswer>, IAnswer> formatter)
		{
			Statements = await _whereTask.ConfigureAwait(false);

			await ProcessChildrenIfNeedAsync().ConfigureAwait(false);

			var answer = formatter(Context, Statements, ChildAnswers);

			answer.Explanation.Expand(AdditionalTransitives);

			return answer;
		}

		public async Task<IAnswer> SelectAllConceptsAsync(
			Func<StatementT, IConcept> resultConceptSelector,
			Func<QuestionT, IConcept> titleConceptSelector,
			String titleConceptCaption,
			Func<ILanguage, String> answerFormat,
			Func<IEnumerable<IConcept>, IEnumerable<IConcept>> conceptsFilter = null)
		{
			Statements = await _whereTask.ConfigureAwait(false);

			await ProcessChildrenIfNeedAsync().ConfigureAwait(false);

			if (await Statements.AnyAsync().ConfigureAwait(false))
			{
				if (conceptsFilter == null)
				{
					conceptsFilter = concepts => concepts;
				}

				var resultConcepts = await conceptsFilter(Statements.Select(resultConceptSelector)).ToListAsync().ConfigureAwait(false);

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

		public async Task<IAnswer> SelectFirstConceptAsync(
			Func<StatementT, IConcept> resultConceptSelector,
			Func<ILanguage, String> answerFormat,
			Func<StatementT, IDictionary<String, IKnowledge>> getParameters)
		{
			Statements = await _whereTask.ConfigureAwait(false);

			await ProcessChildrenIfNeedAsync().ConfigureAwait(false);

			IAnswer answer = null;

			var statement = await Statements.FirstOrDefaultAsync().ConfigureAwait(false);
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
				var childAnswer = await ChildAnswers.FirstOrDefaultAsync().ConfigureAwait(false);
				if (childAnswer != null)
				{
					childAnswer.Answer.Explanation.Expand(childAnswer.TransitiveStatements);
					answer = childAnswer.Answer;
				}
			}

			return answer ?? Answer.CreateUnknown();
		}

		public async Task<BooleanAnswer> SelectBooleanAsync(
			Predicate<ICollection<StatementT>> valueGetter,
			Func<ILanguage, String> trueFormat,
			Func<ILanguage, String> falseFormat,
			IDictionary<String, IKnowledge> parameters)
		{
			Statements = await _whereTask.ConfigureAwait(false);

			await ProcessChildrenIfNeedAsync().ConfigureAwait(false);

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

		public async Task<BooleanAnswer> SelectBooleanIncludingChildrenAsync(
			Predicate<ICollection<StatementT>> valueGetter,
			Func<ILanguage, String> trueFormat,
			Func<ILanguage, String> falseFormat,
			IDictionary<String, IKnowledge> parameters)
		{
			Statements = await _whereTask.ConfigureAwait(false);

			await ProcessChildrenIfNeedAsync();

			Boolean result = false;
			var explanation = new List<IStatement>(Statements.OfType<IStatement>());

			if (valueGetter(Statements))
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

		public async Task<StatementsAnswer<StatementT>> SelectStatementsAsync()
		{
			Statements = await _whereTask;

			var format = new UnstructuredContainer(new FormattedText(
				language => language.Statements.FoundStatements,
				new Dictionary<String, IKnowledge>()))
				.AppendBulletsList(Statements.OfType<IKnowledge>().Enumerate());

			return new StatementsAnswer<StatementT>(
				Statements,
				format,
				new Explanation(Statements));
		}

		protected virtual async Task ProcessChildrenIfNeedAsync()
		{
			if (await NeedToProcessTransitives(Statements).ConfigureAwait(false))
			{
				var transitives = new Dictionary<NestedQuestion, Task<IAnswer>>();
				foreach (var transitive in GetTransitiveQuestions(Context))
				{
					transitives[transitive] = transitive.Question.AskAsync(Context);
				}

				await Task.WhenAll(transitives.Values).ConfigureAwait(false);

				ChildAnswers = new List<ChildAnswer>();
				foreach (var transitive in transitives)
				{
					var answer = transitive.Value.Await();
					if (!answer.IsEmpty)
					{
						ChildAnswers.Add(new ChildAnswer(transitive.Key.Question, answer, transitive.Key.TransitiveStatements));
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
				.Select(questionContext => getQuestionSubject(questionContext.Question)).ToList());

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