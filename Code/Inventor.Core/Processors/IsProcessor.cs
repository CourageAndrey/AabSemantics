using System;
using System.Collections.Generic;

using Inventor.Core.Base;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class IsProcessor : QuestionProcessor<IsQuestion>
	{
		public override IAnswer Process(IQuestionProcessingContext<IsQuestion> context)
		{
			var question = context.QuestionX;
			var explanation = new List<IsStatement>();
			Boolean yes = context.KnowledgeBase.Statements.GetParentsAllLevels(question.ChildConcept, explanation).Contains(question.ParentConcept);
			return new Answer(
				yes,
				new FormattedText(
					yes ? new Func<String>(() => context.Language.Answers.IsTrue) : () => context.Language.Answers.IsFalse,
					new Dictionary<String, INamed>
					{
						{ "#CHILD#", question.ChildConcept },
						{ "#PARENT#", question.ParentConcept },
					}),
				new Explanation(explanation));
		}
	}
}
