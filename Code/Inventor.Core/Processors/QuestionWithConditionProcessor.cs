using Inventor.Core.Base;
using Inventor.Core.Questions;

namespace Inventor.Core.Processors
{
	public sealed class QuestionWithConditionProcessor : QuestionProcessor<QuestionWithCondition>
	{
		public override IAnswer Process(IQuestionProcessingContext<QuestionWithCondition> context)
		{
			var question = context.QuestionX;
			using (var nestedContext = context.AskQuestion(question.Question))
			{
				foreach (var knowledge in question.Conditions)
				{
					context.KnowledgeBase.Statements.Add(knowledge);
					knowledge.Context = nestedContext;
				}

				var questionType = question.Question.GetType();
				var questionRepository = context.QuestionRepository;
				var questionProcessor = questionRepository.QuestionDefinitions[questionType].CreateProcessor();
				return questionProcessor.Process(nestedContext);
			}
		}
	}
}
