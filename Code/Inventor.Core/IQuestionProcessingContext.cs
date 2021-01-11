using Inventor.Core.Base;

namespace Inventor.Core
{
	public interface IQuestionProcessingContext
	{
		IKnowledgeBase KnowledgeBase
		{ get; }

		IQuestion Question
		{ get; }

		IQuestionRepository QuestionRepository
		{ get; }

		ILanguage Language
		{ get; }
	}

	public interface IQuestionProcessingContext<out QuestionT> : IQuestionProcessingContext
		where QuestionT : IQuestion
	{
		QuestionT QuestionX
		{ get; }
	}

	public static class ProcessingContextHelper
	{
		public static IQuestionProcessingContext<QuestionT> GetExplicit<QuestionT>(this IQuestionProcessingContext context)
			where QuestionT : IQuestion
		{
			return new QuestionProcessingContext<QuestionT>(
				context.KnowledgeBase,
				(QuestionT) context.Question,
				context.QuestionRepository,
				context.Language);
		}
	}
}
