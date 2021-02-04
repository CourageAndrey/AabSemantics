using System;
using System.Collections.Generic;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	[Obsolete("This class will be removed as soon as QuestionDialog supports CheckStatementQuestion. Please, use CheckStatementQuestion with corresponding statement instead.")]
	public sealed class IsProcessor : QuestionProcessor<IsQuestion>
	{
		public override IAnswer Process(IQuestionProcessingContext<IsQuestion> context)
		{
			var question = context.Question;
			var activeContexts = context.GetHierarchy();

			var explanation = new List<IsStatement>();
			Boolean yes = context.KnowledgeBase.Statements.Enumerate(activeContexts).GetParentsAllLevels(question.Child, explanation).Contains(question.Parent);
			return new BooleanAnswer(
				yes,
				new FormattedText(
					yes ? new Func<String>(() => context.Language.Answers.IsTrue) : () => context.Language.Answers.IsFalse,
					new Dictionary<String, INamed>
					{
						{ Strings.ParamParent, question.Child },
						{ Strings.ParamChild, question.Parent },
					}),
				new Explanation(explanation));
		}
	}
}
