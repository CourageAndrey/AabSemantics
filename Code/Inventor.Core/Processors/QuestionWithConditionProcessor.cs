using Inventor.Core.Base;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class QuestionWithConditionProcessor : QuestionProcessor<QuestionWithCondition>
	{
		public override IAnswer Process(IQuestionProcessingContext<QuestionWithCondition> context)
		{
			var question = context.QuestionX;
			return question.Question.Ask(context, question.Conditions);
		}
	}
}
