using System;

using Inventor.Core.Base;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	[Obsolete("As all questions have their own precondition list, this class becomes unusable. It will be deleted after UI will support proconditions for all questions.")]
	public sealed class QuestionWithConditionProcessor : QuestionProcessor<QuestionWithCondition>
	{
		public override IAnswer Process(IQuestionProcessingContext<QuestionWithCondition> context)
		{
			var question = context.Question;
			return question.Question.Ask(context, question.Preconditions);
		}
	}
}
