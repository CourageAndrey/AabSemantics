using System;
using System.Collections.Generic;
using System.Linq;
using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	[Obsolete("This class will be removed as soon as QuestionDialog supports CheckStatementQuestion. Please, use CheckStatementQuestion with corresponding statement instead.")]
	public sealed class IsProcessor : QuestionProcessor<IsQuestion, IsStatement>
	{
		protected override IAnswer CreateAnswer(IQuestionProcessingContext<IsQuestion> context, ICollection<IsStatement> statements)
		{
			Boolean yes = statements.Any();
			return new BooleanAnswer(
				yes,
				new FormattedText(
					yes ? new Func<String>(() => context.Language.Answers.IsTrue) : () => context.Language.Answers.IsFalse,
					new Dictionary<String, INamed>
					{
						{ Strings.ParamParent, context.Question.Child },
						{ Strings.ParamChild, context.Question.Parent },
					}),
				new Explanation(statements));
		}

		protected override Boolean DoesStatementMatch(IQuestionProcessingContext<IsQuestion> context, IsStatement statement)
		{
			return statement.Parent == context.Question.Parent && statement.Child == context.Question.Child;
		}

		protected override IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<IsQuestion> context)
		{
			var activeContexts = context.GetHierarchy();
			var alreadyViewedConcepts = new HashSet<IConcept>(activeContexts.OfType<IQuestionProcessingContext<IsQuestion>>().Select(questionContext => questionContext.Question.Child));

			var question = context.Question;
			var transitiveStatements = context.KnowledgeBase.Statements.Enumerate<IsStatement>(activeContexts).Where(isStatement => isStatement.Child == question.Child);

			foreach (var transitiveStatement in transitiveStatements)
			{
				var parent = transitiveStatement.Parent;
				if (!alreadyViewedConcepts.Contains(parent))
				{
					yield return new NestedQuestion(new IsQuestion(parent, question.Parent), new IStatement[] { transitiveStatement });
				}
			}
		}
	}
}
