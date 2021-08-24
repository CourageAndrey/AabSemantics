using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;

namespace Inventor.Core.Questions
{
	public class StatementQuestionProcessor<QuestionT, StatementT>
		where QuestionT : IQuestion
		where StatementT : IStatement
	{
		#region Properties

		public IQuestionProcessingContext<QuestionT> Context
		{ get; }

		public ICollection<StatementT> Statements
		{ get; private set; }

		public ICollection<ChildAnswer> ChildAnswers
		{ get; private set; }

		public ICollection<IStatement> AdditionalTransitives
		{ get; private set; }

		public IAnswer Answer
		{ get; private set; }

		private Func<ICollection<StatementT>, Boolean> _needToProcessTransitives = statements => false;
		private Func<IQuestionProcessingContext<QuestionT>, IEnumerable<NestedQuestion>> _getTransitives = context => Array.Empty<NestedQuestion>();
		private Boolean _needToAggregateTransitivesToStatements;

		#endregion

		public StatementQuestionProcessor(IQuestionProcessingContext context)
		{
			Context = (IQuestionProcessingContext<QuestionT>) context;
			Statements = Array.Empty<StatementT>();
			ChildAnswers = Array.Empty<ChildAnswer>();
			AdditionalTransitives = Array.Empty<IStatement>();
			Answer = Answers.Answer.CreateUnknown(Context.Language);
		}

		public StatementQuestionProcessor<QuestionT, StatementT> Where(Func<StatementT, Boolean> match)
		{
			Statements = Context.SemanticNetwork.Statements
				.Enumerate<StatementT>(Context.ActiveContexts)
				.Where(match)
				.ToList();

			return this;
		}

		protected virtual void ProcessChildrenIfNeed()
		{
			if (_needToProcessTransitives(Statements))
			{
				ChildAnswers = new List<ChildAnswer>();
				foreach (var nested in _getTransitives(Context))
				{
					var answer = nested.Question.Ask(Context);
					if (!answer.IsEmpty)
					{
						ChildAnswers.Add(new ChildAnswer(nested.Question, answer, nested.TransitiveStatements));
					}
				}

				if (_needToAggregateTransitivesToStatements)
				{
					DoAggregateTransitivesToStatements();
				}
			}
			else
			{
				ChildAnswers = Array.Empty<ChildAnswer>();
			}
		}

		public StatementQuestionProcessor<QuestionT, StatementT> WithTransitives(
			Func<ICollection<StatementT>, Boolean> needToProcess,
			Func<IQuestionProcessingContext<QuestionT>, IEnumerable<NestedQuestion>> getNestedQuestions)
		{
			_needToProcessTransitives = needToProcess;
			_getTransitives = getNestedQuestions;
			return this;
		}

		public StatementQuestionProcessor<QuestionT, StatementT> WithTransitives(
			Func<ICollection<StatementT>, Boolean> needToProcess,
			Func<QuestionT, IConcept> getQuestionSubject,
			Func<IConcept, QuestionT> createQuestionForSubject)
		{
			return WithTransitives(needToProcess, context => GetNestedQuestions(getQuestionSubject, createQuestionForSubject));
		}

		private IEnumerable<NestedQuestion> GetNestedQuestions(
			Func<QuestionT, IConcept> getQuestionSubject,
			Func<IConcept, QuestionT> createQuestionForSubject)
		{
			var alreadyViewedConcepts = new HashSet<IConcept>(Context.ActiveContexts
				.OfType<IQuestionProcessingContext<QuestionT>>()
				.Select(questionContext => getQuestionSubject(questionContext.Question)));

			var question = Context.Question;
			var subject = getQuestionSubject(question);
			var transitiveStatements = Context.SemanticNetwork.Statements
				.Enumerate<Statements.IsStatement>(Context.ActiveContexts)
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

		public StatementQuestionProcessor<QuestionT, StatementT> Select(Func<IQuestionProcessingContext<QuestionT>, ICollection<StatementT>, ICollection<ChildAnswer>, IAnswer> formatter)
		{
			ProcessChildrenIfNeed();

			Answer = formatter(Context, Statements, ChildAnswers);

			AppendAdditionalTransitives();

			return this;
		}

		public StatementQuestionProcessor<QuestionT, StatementT> SelectAllConcepts(
			Func<StatementT, IConcept> resultConceptSelector,
			Func<QuestionT, IConcept> titleConceptSelector,
			String titleConceptCaption,
			Func<ILanguage, String> answerFormat)
		{
			ProcessChildrenIfNeed();

			if (Statements.Any())
			{
				var resultConcepts = Statements.Select(resultConceptSelector).ToList();

				String format;
				var parameters = resultConcepts.Enumerate(out format);
				parameters.Add(titleConceptCaption, titleConceptSelector(Context.Question));

				Answer = new ConceptsAnswer(
					resultConcepts,
					new FormattedText(() => answerFormat(Context.Language) + format + ".", parameters),
					new Explanation(Statements.OfType<IStatement>()));

				AppendAdditionalTransitives();
			}

			return this;
		}

		public StatementQuestionProcessor<QuestionT, StatementT> SelectFirstConcept(
			Func<StatementT, IConcept> resultConceptSelector,
			Func<ILanguage, String> answerFormat,
			Func<StatementT, IDictionary<String, INamed>> getParameters)
		{
			ProcessChildrenIfNeed();

			var statement = Statements.FirstOrDefault();
			if (statement != null)
			{
				Answer = new ConceptAnswer(
					resultConceptSelector(statement),
					new FormattedText(
						() => answerFormat(Context.Language),
						getParameters(statement)),
					new Explanation(Statements.OfType<IStatement>()));

				AppendAdditionalTransitives();
			}

			IfEmptyTrySelectFirstChild();

			return this;
		}

		public StatementQuestionProcessor<QuestionT, StatementT> SelectBoolean(
			Func<ICollection<StatementT>, Boolean> valueGetter,
			Func<ILanguage, String> trueFormat,
			Func<ILanguage, String> falseFormat,
			IDictionary<String, INamed> parameters)
		{
			ProcessChildrenIfNeed();

			Boolean value = valueGetter(Statements);

			Answer = new BooleanAnswer(
				value,
				new FormattedText(
					value ? new Func<String>(() => trueFormat(Context.Language)) : () => falseFormat(Context.Language),
					parameters),
				new Explanation(Statements.OfType<IStatement>()));

			AppendAdditionalTransitives();

			return this;
		}

		public StatementQuestionProcessor<QuestionT, StatementT> SelectBooleanIncludingChildren(
			Func<ICollection<StatementT>, Boolean> valueGetter,
			Func<ILanguage, String> trueFormat,
			Func<ILanguage, String> falseFormat,
			IDictionary<String, INamed> parameters)
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

			Answer = new BooleanAnswer(
				result,
				new FormattedText(
					result ? new Func<String>(() => trueFormat(Context.Language)) : () => falseFormat(Context.Language),
					parameters),
				new Explanation(explanation));

			AppendAdditionalTransitives();

			return this;
		}

		protected virtual void IfEmptyTrySelectFirstChild()
		{
			if (Answer.IsEmpty)
			{
				var childAnswer = ChildAnswers.FirstOrDefault();
				if (childAnswer != null)
				{
					childAnswer.Answer.Explanation.Expand(childAnswer.TransitiveStatements);
					Answer = childAnswer.Answer;
				}
			}
		}

		public StatementQuestionProcessor<QuestionT, StatementT> AggregateTransitivesToStatements()
		{
			_needToAggregateTransitivesToStatements = true;
			return this;
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

		protected virtual void AppendAdditionalTransitives()
		{
			Answer.Explanation.Expand(AdditionalTransitives);
		}
	}
}