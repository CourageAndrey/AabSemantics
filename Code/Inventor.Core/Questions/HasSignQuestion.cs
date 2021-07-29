using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class HasSignQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public IConcept Sign
		{ get; }

		public Boolean Recursive
		{ get; }

		#endregion

		public HasSignQuestion(IConcept concept, IConcept sign, Boolean recursive, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));
			if (sign == null) throw new ArgumentNullException(nameof(sign));

			Concept = concept;
			Sign = sign;
			Recursive = recursive;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<HasSignQuestion, HasSignStatement>(DoesStatementMatch)
				.ProcessTransitives(NeedToCheckTransitives, GetNestedQuestions)
				.Select(CreateAnswer);
		}

		private IAnswer CreateAnswer(IQuestionProcessingContext<HasSignQuestion> context, ICollection<HasSignStatement> statements, ICollection<ChildAnswer> childAnswers)
		{
			Boolean result = false;
			var explanation = new List<IStatement>(statements);

			if (statements.Count > 0)
			{
				result = true;
			}

			foreach (var childAnswer in childAnswers)
			{
				if (((BooleanAnswer) childAnswer.Answer).Result)
				{
					result = true;
					explanation.AddRange(childAnswer.Answer.Explanation.Statements);
					explanation.AddRange(childAnswer.TransitiveStatements);
				}
			}

			return new BooleanAnswer(
				result,
				new FormattedText(
					() => String.Format(result ? context.Language.Answers.HasSignTrue : context.Language.Answers.HasSignFalse, Recursive ? context.Language.Answers.RecursiveTrue : context.Language.Answers.RecursiveFalse),
					new Dictionary<String, INamed>
					{
						{ Strings.ParamConcept, Concept },
						{ Strings.ParamSign, Sign },
					}),
				new Explanation(explanation));
		}

		private Boolean DoesStatementMatch(HasSignStatement statement)
		{
			return statement.Concept == Concept && statement.Sign == Sign;
		}

		private Boolean NeedToCheckTransitives(ICollection<HasSignStatement> statements)
		{
			return statements.Count == 0 && Recursive;
		}

		private IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<HasSignQuestion> context)
		{
			if (!Recursive) yield break;

			var alreadyViewedConcepts = new HashSet<IConcept>(context.ActiveContexts.OfType<IQuestionProcessingContext<HasSignQuestion>>().Select(questionContext => questionContext.Question.Concept));

			var question = context.Question;
			var transitiveStatements = context.KnowledgeBase.Statements.Enumerate<IsStatement>(context.ActiveContexts).Where(isStatement => isStatement.Child == question.Concept);

			foreach (var transitiveStatement in transitiveStatements)
			{
				var parent = transitiveStatement.Parent;
				if (!alreadyViewedConcepts.Contains(parent))
				{
					yield return new NestedQuestion(new HasSignQuestion(parent, question.Sign, true), new IStatement[] { transitiveStatement });
				}
			}
		}
	}
}
