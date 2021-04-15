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
	public sealed class IsPartOfProcessor : QuestionProcessor<IsPartOfQuestion, HasPartStatement>
	{
		public override IAnswer Process(IQuestionProcessingContext<IsPartOfQuestion> context)
		{
			var activeContexts = context.GetHierarchy();
			var statements = context.KnowledgeBase.Statements.Enumerate<HasPartStatement>(activeContexts).Where(statement => DoesStatementMatch(context, statement)).ToList();
			return CreateAnswer(context, statements);
		}

		protected override IAnswer CreateAnswer(IQuestionProcessingContext<IsPartOfQuestion> context, ICollection<HasPartStatement> statements)
		{
			return new BooleanAnswer(
				statements.Any(),
				new FormattedText(statements.Any() ? new Func<String>(() => context.Language.Answers.IsPartOfTrue) : () => context.Language.Answers.IsPartOfFalse, new Dictionary<String, INamed>
				{
					{ Strings.ParamParent, context.Question.Parent },
					{ Strings.ParamChild, context.Question.Child },
				}),
				new Explanation(statements));
		}

		protected override Boolean DoesStatementMatch(IQuestionProcessingContext<IsPartOfQuestion> context, HasPartStatement statement)
		{
			return statement.Whole == context.Question.Parent && statement.Part == context.Question.Child;
		}
	}
}
