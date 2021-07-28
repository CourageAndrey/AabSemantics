using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IQuestion
	{
		ICollection<IStatement> Preconditions
		{ get; }
	}

	public interface IQuestion<StatementT> : IQuestion
		where StatementT : IStatement
	{
	}

	public static class QuestionHelper
	{
		public static IAnswer Ask(this IQuestion question, IKnowledgeBaseContext context)
		{
			using (var questionContext = context.CreateQuestionContext(question))
			{
				var questionType = question.GetType();
				var questionRepository = context.QuestionRepository;
				var questionDefinition = questionRepository.QuestionDefinitions[questionType];
				var questionProcessor = questionDefinition.CreateProcessor();
				return questionProcessor.Process(questionContext);
			}
		}
	}
}
