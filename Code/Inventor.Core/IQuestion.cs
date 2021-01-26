using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IQuestion
	{
	}

	public static class QuestionHelper
	{
		public static IAnswer Ask(this IQuestion question, IKnowledgeBaseContext context, IEnumerable<IStatement> additionalStatements = null)
		{
			using (var questionContext = context.CreateQuestionContext(question))
			{
				foreach (var statement in additionalStatements ?? new IStatement[0])
				{
					statement.Context = questionContext;
					context.KnowledgeBase.Statements.Add(statement);
				}

				var questionType = question.GetType();
				var questionRepository = context.QuestionRepository;
				var questionProcessor = questionRepository.QuestionDefinitions[questionType].CreateProcessor();
				return questionProcessor.Process(questionContext);
			}
		}
	}
}
