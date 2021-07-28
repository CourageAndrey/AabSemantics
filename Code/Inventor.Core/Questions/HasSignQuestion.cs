using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class HasSignQuestion : Question<HasSignQuestion, HasSignStatement>
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

		protected override IAnswer CreateAnswer(IQuestionProcessingContext<HasSignQuestion> context, ICollection<HasSignStatement> statements)
		{
			return new BooleanAnswer(
				statements.Any(),
				new FormattedText(
					() => String.Format(statements.Any() ? context.Language.Answers.HasSignTrue : context.Language.Answers.HasSignFalse, Recursive ? context.Language.Answers.RecursiveTrue : context.Language.Answers.RecursiveFalse),
					new Dictionary<String, INamed>
					{
						{ Strings.ParamConcept, Concept },
						{ Strings.ParamSign, Sign },
					}),
				new Explanation(statements));
		}

		protected override Boolean DoesStatementMatch(HasSignStatement statement)
		{
			return statement.Concept == Concept && statement.Sign == Sign;
		}

		protected override Boolean NeedToCheckTransitives(ICollection<HasSignStatement> statements)
		{
			return Recursive;
		}

		protected override IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<HasSignQuestion> context)
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

		protected override IAnswer ProcessChildAnswers(IQuestionProcessingContext<HasSignQuestion> questionProcessingContext, ICollection<HasSignStatement> statements, ICollection<ChildAnswer> childAnswers)
		{
			var resultStatements = new List<HasSignStatement>(statements);
			var additionalStatements = new List<IStatement>();

			foreach (var childAnswer in childAnswers)
			{
				if (((BooleanAnswer) childAnswer.Answer).Result)
				{
					var answerStatements = childAnswer.Answer.Explanation.Statements.OfType<HasSignStatement>().ToList();
					resultStatements.AddRange(answerStatements);
					additionalStatements.AddRange(childAnswer.Answer.Explanation.Statements.Except(answerStatements));
					additionalStatements.AddRange(childAnswer.TransitiveStatements);
				}
			}

			var result = CreateAnswer(questionProcessingContext, resultStatements);
			result.Explanation.Expand(additionalStatements);
			return result;
		}
	}
}
