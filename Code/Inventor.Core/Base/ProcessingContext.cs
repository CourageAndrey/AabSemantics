namespace Inventor.Core.Base
{
	public class ProcessingContext : IProcessingContext
	{
		#region Properties

		public IKnowledgeBase KnowledgeBase
		{ get; }

		public IQuestion Question
		{ get; }

		public IQuestionRepository QuestionRepository
		{ get; }

		public ILanguage Language
		{ get; }

		#endregion

		public ProcessingContext(IKnowledgeBase knowledgeBase, IQuestion question, IQuestionRepository questionRepository, ILanguage language)
		{
			KnowledgeBase = knowledgeBase;
			Question = question;
			QuestionRepository = questionRepository;
			Language = language;
		}
	}

	public class ProcessingContext<QuestionT> : ProcessingContext, IProcessingContext<QuestionT>
		where QuestionT : IQuestion
	{
		public QuestionT QuestionX
		{ get; }

		public ProcessingContext(IKnowledgeBase knowledgeBase, QuestionT question, IQuestionRepository questionRepository, ILanguage language)
			: base(knowledgeBase, question, questionRepository, language)
		{
			QuestionX = question;
		}
	}
}
