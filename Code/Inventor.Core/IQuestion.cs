using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IQuestion
	{
		ICollection<IStatement> Preconditions
		{ get; }
	}

	public static class QuestionHelper
	{
		public static IAnswer Ask(this IQuestion question, IKnowledgeBaseContext context)
		{
			using (var questionContext = context.CreateQuestionContext(question))
			{
				var questionType = question.GetType();
				var questionRepository = context.QuestionRepository;
				var questionProcessor = questionRepository.QuestionDefinitions[questionType].CreateProcessor();
				return questionProcessor.Process(questionContext);
			}
		}
	}
}
