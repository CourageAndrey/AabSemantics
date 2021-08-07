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
		{ get; }

		public ICollection<ChildAnswer> ChildAnswers
		{ get; private set; }

		public IAnswer Answer
		{ get; private set; }

		#endregion

		public StatementQuestionProcessor(IQuestionProcessingContext context, Func<StatementT, Boolean> match)
		{
			Context = (IQuestionProcessingContext<QuestionT>) context;

			Statements = context.SemanticNetwork.Statements
				.Enumerate<StatementT>(context.ActiveContexts)
				.Where(match)
				.ToList();

			ChildAnswers = new ChildAnswer[0];
		}

		public StatementQuestionProcessor<QuestionT, StatementT> ProcessTransitives(
			Func<ICollection<StatementT>, Boolean> needToProcess,
			Func<IQuestionProcessingContext<QuestionT>, IEnumerable<NestedQuestion>> getNestedQuestions)
		{
			if (needToProcess(Statements))
			{
				ChildAnswers = new List<ChildAnswer>();
				foreach (var nested in getNestedQuestions(Context))
				{
					var answer = nested.Question.Ask(Context);
					if (!answer.IsEmpty)
					{
						ChildAnswers.Add(new ChildAnswer(nested.Question, answer, nested.TransitiveStatements));
					}
				}
			}
			else
			{
				ChildAnswers = new ChildAnswer[0];
			}
			return this;
		}

		public StatementQuestionProcessor<QuestionT, StatementT> Select(Func<IQuestionProcessingContext<QuestionT>, ICollection<StatementT>, ICollection<ChildAnswer>, IAnswer> formatter)
		{
			Answer = formatter(Context, Statements, ChildAnswers);
			return this;
		}

		public StatementQuestionProcessor<QuestionT, StatementT> SelectConcepts(
			Func<StatementT, IConcept> resultConceptSelector,
			Func<QuestionT, IConcept> titleConceptSelector,
			String titleConceptCaption,
			Func<ILanguage, String> answerFormat)
		{
			var language = Context.Language;
			if (Statements.Any())
			{
				var resultConcepts = Statements.Select(resultConceptSelector).ToList();

				String format;
				var parameters = resultConcepts.Enumerate(out format);
				parameters.Add(titleConceptCaption, titleConceptSelector(Context.Question));

				Answer = new ConceptsAnswer(
					resultConcepts,
					new FormattedText(() => answerFormat(language) + format + ".", parameters),
					new Explanation(Statements.OfType<IStatement>()));
			}
			else
			{
				var childAnswer = ChildAnswers.FirstOrDefault();
				if (childAnswer != null)
				{
					childAnswer.Answer.Explanation.Expand(childAnswer.TransitiveStatements);
					Answer = childAnswer.Answer;
				}
			}

			if (Answer == null)
			{
				Answer = Answers.Answer.CreateUnknown(language);
			}

			return this;
		}
	}
}