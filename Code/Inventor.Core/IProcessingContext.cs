namespace Inventor.Core
{
	public interface IProcessingContext
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

	public interface IProcessingContext<out QuestionT> : IProcessingContext
		where QuestionT : Question
	{
		QuestionT QuestionX
		{ get; }
	}

	public static class ProcessingContextHelper
	{
		public static IProcessingContext<QuestionT> GetExplicit<QuestionT>(this IProcessingContext processingContext)
			where QuestionT : Question
		{
			return new ProcessingContext<QuestionT>(
				processingContext.KnowledgeBase,
				(QuestionT) processingContext.Question,
				processingContext.QuestionRepository,
				processingContext.Language);
		}
	}
}
