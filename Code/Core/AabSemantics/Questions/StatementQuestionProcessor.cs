using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AabSemantics.Answers;
using AabSemantics.Text.Containers;
using AabSemantics.Text.Primitives;
using AabSemantics.Utils;

namespace AabSemantics.Questions
{
	/// <summary>
	/// Fluent pipeline for answering a question from statements of one type: filter with
	/// <see cref="Where"/>, optionally configure transitive lookup with <c>WithTransitives</c>,
	/// then finish with one of the <c>Select*</c> methods, which is what actually runs the query.
	/// <para>
	/// Transitive lookup is how the engine reaches beyond direct statements: when the direct ones
	/// do not settle the question, it asks the same question of related concepts and folds the
	/// child answers in.
	/// </para>
	/// </summary>
	/// <typeparam name="QuestionT">Question type being answered.</typeparam>
	/// <typeparam name="StatementT">Statement type the answer is derived from.</typeparam>
	public class StatementQuestionProcessor<QuestionT, StatementT>
		where QuestionT : IQuestion
		where StatementT : class, IStatement
	{
		#region Properties

		/// <summary>Context being searched, with the question strongly typed.</summary>
		protected IQuestionProcessingContext<QuestionT> Context
		{ get; }

		/// <summary>Statements that passed the filter; populated when a <c>Select*</c> method runs.</summary>
		protected ICollection<StatementT> Statements
		{ get; private set; }

		/// <summary>Answers to the nested questions, if transitive lookup took place.</summary>
		protected ICollection<ChildAnswer> ChildAnswers
		{ get; private set; }

		/// <summary>Statements gathered on the way to the nested questions; merged into the explanation.</summary>
		protected ICollection<IStatement> AdditionalTransitives
		{ get; private set; }

		/// <summary>Decides, from the directly matched statements, whether transitive lookup is needed.</summary>
		protected Func<ICollection<StatementT>, Task<Boolean>> NeedToProcessTransitives
		{ get; private set; }

		/// <summary>Produces the nested questions to ask when transitive lookup is needed.</summary>
		protected Func<IQuestionProcessingContext<QuestionT>, IEnumerable<NestedQuestion>> GetTransitiveQuestions
		{ get; private set; }

		/// <summary>Whether statements found by the nested questions are added to <see cref="Statements"/>.</summary>
		protected Boolean NeedToAggregateTransitivesToStatements
		{ get; private set; }

		/// <summary>
		/// Cancels this query, taken from the question context. Observed while filtering statements
		/// and passed on to every nested question.
		/// </summary>
		protected CancellationToken CancellationToken
		{ get { return Context.CancellationToken; } }

		private Task<List<StatementT>> _whereTask;

		#endregion

		/// <summary>Creates a processor with no filter and transitive lookup disabled.</summary>
		/// <param name="context">Question context to search; must be typed for <typeparamref name="QuestionT"/>.</param>
		/// <exception cref="InvalidCastException">The context is for a different question type.</exception>
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

		/// <summary>
		/// Selects the statements to answer from, restricted to the context and its ancestors.
		/// The query starts immediately but is only awaited by the closing <c>Select*</c> call.
		/// </summary>
		/// <param name="match">Predicate a statement must satisfy.</param>
		/// <returns>This processor, to allow call chaining.</returns>
		public StatementQuestionProcessor<QuestionT, StatementT> Where(Func<StatementT, Boolean> match)
		{
			_whereTask = Context.SemanticNetwork.Statements
				.Enumerate<StatementT>(Context.ActiveContexts)
				.Where(match)
				.ToListAsync(CancellationToken);

			return this;
		}

		/// <summary>Enables transitive lookup, with the nested questions supplied explicitly.</summary>
		/// <param name="needToProcessTransitives">Decides from the direct statements whether to recurse.</param>
		/// <param name="getTransitiveQuestions">Produces the nested questions to ask.</param>
		/// <param name="needToAggregateTransitivesToStatements">Whether to merge the children's statements into this answer's.</param>
		/// <returns>This processor, to allow call chaining.</returns>
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

		/// <summary>
		/// Enables transitive lookup along the subject's classification hierarchy: the same
		/// question is re-asked of each parent of the question's subject.
		/// </summary>
		/// <param name="needToProcessTransitives">Decides from the direct statements whether to recurse.</param>
		/// <param name="getQuestionSubject">Reads the subject concept out of the question.</param>
		/// <param name="createQuestionForSubject">Builds the same kind of question for another subject.</param>
		/// <param name="needToAggregateTransitivesToStatements">Whether to merge the children's statements into this answer's.</param>
		/// <returns>This processor, to allow call chaining.</returns>
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

		/// <summary>
		/// Runs the query and hands the raw results to a caller-supplied formatter, for answers
		/// that none of the other <c>Select*</c> methods can shape.
		/// </summary>
		/// <param name="formatter">Builds the answer from the context, the matched statements and the child answers.</param>
		/// <returns>The formatter's answer, with the transitive statements merged into its explanation.</returns>
		public async Task<IAnswer> SelectCustomAsync(Func<IQuestionProcessingContext<QuestionT>, ICollection<StatementT>, ICollection<ChildAnswer>, IAnswer> formatter)
		{
			Statements = await _whereTask.ConfigureAwait(false);

			await ProcessChildrenIfNeedAsync().ConfigureAwait(false);

			var answer = formatter(Context, Statements, ChildAnswers);

			answer.Explanation.Expand(AdditionalTransitives);

			return answer;
		}

		/// <summary>Answers with the concepts taken from every matched statement, rendered as a bullet list.</summary>
		/// <param name="resultConceptSelector">Picks the concept to report out of a statement.</param>
		/// <param name="titleConceptSelector">Picks the concept the question is about, named in the caption.</param>
		/// <param name="titleConceptCaption">Anchor name the title concept is substituted under.</param>
		/// <param name="answerFormat">Selects the caption's format string from a language.</param>
		/// <param name="conceptsFilter">Post-processes the concept list, e.g. to deduplicate; identity when <c>null</c>.</param>
		/// <returns>A concept-list answer, or the "unknown" answer when nothing matched.</returns>
		public async Task<IAnswer> SelectAllConceptsAsync(
			Func<StatementT, IConcept> resultConceptSelector,
			Func<QuestionT, IConcept> titleConceptSelector,
			String titleConceptCaption,
			Func<ILanguage, String> answerFormat,
			Func<IEnumerable<IConcept>, IEnumerable<IConcept>> conceptsFilter = null)
		{
			Statements = await _whereTask.ConfigureAwait(false);

			await ProcessChildrenIfNeedAsync().ConfigureAwait(false);

			if (await Statements.AnyAsync(cancellationToken: CancellationToken).ConfigureAwait(false))
			{
				if (conceptsFilter == null)
				{
					conceptsFilter = concepts => concepts;
				}

				var resultConcepts = await conceptsFilter(Statements.Select(resultConceptSelector)).ToListAsync(CancellationToken).ConfigureAwait(false);

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

		/// <summary>
		/// Answers with the concept from the first matched statement, falling back to the first
		/// child answer when nothing matched directly.
		/// </summary>
		/// <param name="resultConceptSelector">Picks the concept to report out of a statement.</param>
		/// <param name="answerFormat">Selects the answer's format string from a language.</param>
		/// <param name="getParameters">Supplies the knowledge items the format string refers to by anchor.</param>
		/// <returns>A single-concept answer, or the "unknown" answer when neither source yielded anything.</returns>
		public async Task<IAnswer> SelectFirstConceptAsync(
			Func<StatementT, IConcept> resultConceptSelector,
			Func<ILanguage, String> answerFormat,
			Func<StatementT, IDictionary<String, IKnowledge>> getParameters)
		{
			Statements = await _whereTask.ConfigureAwait(false);

			await ProcessChildrenIfNeedAsync().ConfigureAwait(false);

			IAnswer answer = null;

			var statement = await Statements.FirstOrDefaultAsync(cancellationToken: CancellationToken).ConfigureAwait(false);
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
				var childAnswer = await ChildAnswers.FirstOrDefaultAsync(cancellationToken: CancellationToken).ConfigureAwait(false);
				if (childAnswer != null)
				{
					childAnswer.Answer.Explanation.Expand(childAnswer.TransitiveStatements);
					answer = childAnswer.Answer;
				}
			}

			return answer ?? Answer.CreateUnknown();
		}

		/// <summary>Answers yes or no, judging from the matched statements alone.</summary>
		/// <param name="valueGetter">Derives the yes/no value from the matched statements.</param>
		/// <param name="trueFormat">Selects the affirmative wording from a language.</param>
		/// <param name="falseFormat">Selects the negative wording from a language.</param>
		/// <param name="parameters">Knowledge items the wordings refer to by anchor.</param>
		/// <returns>A yes/no answer; never the "unknown" answer.</returns>
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

		/// <summary>
		/// Answers yes or no, treating a "yes" from any nested question as a "yes" overall —
		/// the disjunction over the direct statements and every child answer.
		/// </summary>
		/// <param name="valueGetter">Derives the yes/no value from the matched statements.</param>
		/// <param name="trueFormat">Selects the affirmative wording from a language.</param>
		/// <param name="falseFormat">Selects the negative wording from a language.</param>
		/// <param name="parameters">Knowledge items the wordings refer to by anchor.</param>
		/// <returns>A yes/no answer whose explanation merges the contributing children's.</returns>
		public async Task<BooleanAnswer> SelectBooleanIncludingChildrenAsync(
			Predicate<ICollection<StatementT>> valueGetter,
			Func<ILanguage, String> trueFormat,
			Func<ILanguage, String> falseFormat,
			IDictionary<String, IKnowledge> parameters)
		{
			Statements = await _whereTask.ConfigureAwait(false);

			await ProcessChildrenIfNeedAsync().ConfigureAwait(false);

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

		/// <summary>
		/// Answers with the matched statements themselves, as a bullet list. Unlike the other
		/// <c>Select*</c> methods this one does not perform transitive lookup.
		/// </summary>
		/// <returns>A statement-list answer; empty when nothing matched.</returns>
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

		/// <summary>
		/// Asks the nested questions when transitive lookup is needed, one after another, and keeps
		/// only the non-empty answers. The first question that fails ends the whole lookup: its
		/// failure is what the caller gets, and the remaining questions are not asked at all.
		/// </summary>
		protected virtual async Task ProcessChildrenIfNeedAsync()
		{
			CancellationToken.ThrowIfCancellationRequested();

			if (await NeedToProcessTransitives(Statements).ConfigureAwait(false))
			{
				var childAnswers = new List<ChildAnswer>();
				foreach (var transitive in GetTransitiveQuestions(Context))
				{
					CancellationToken.ThrowIfCancellationRequested();

					var answer = await transitive.Question.AskAsync(Context, null, CancellationToken).ConfigureAwait(false);
					if (!answer.IsEmpty)
					{
						childAnswers.Add(new ChildAnswer(transitive.Question, answer, transitive.TransitiveStatements));
					}
				}
				ChildAnswers = childAnswers;

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

		/// <summary>
		/// Builds one nested question per parent of the question's subject, following
		/// classification statements. Subjects already asked about further up the context chain
		/// are skipped, which is what stops a classification cycle from recursing forever.
		/// </summary>
		/// <param name="getQuestionSubject">Reads the subject concept out of a question.</param>
		/// <param name="createQuestionForSubject">Builds the same kind of question for another subject.</param>
		/// <returns>Nested questions, each paired with the classification statement that justifies it.</returns>
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

		/// <summary>
		/// Folds the child answers' evidence into this one: statements of the processed type join
		/// <see cref="Statements"/>, the rest become <see cref="AdditionalTransitives"/> so they
		/// still show up in the explanation.
		/// </summary>
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