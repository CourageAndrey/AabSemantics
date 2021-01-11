namespace Inventor.Core.Base
{
	public class QuestionProcessingContext : IQuestionProcessingContext
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

		public QuestionProcessingContext(IKnowledgeBase knowledgeBase, IQuestion question, IQuestionRepository questionRepository, ILanguage language)
		{
			KnowledgeBase = knowledgeBase;
			Question = question;
			QuestionRepository = questionRepository;
			Language = language;
		}
	}

	public class QuestionProcessingContext<QuestionT> : QuestionProcessingContext, IQuestionProcessingContext<QuestionT>
		where QuestionT : IQuestion
	{
		public QuestionT QuestionX
		{ get; }

		public QuestionProcessingContext(IKnowledgeBase knowledgeBase, QuestionT question, IQuestionRepository questionRepository, ILanguage language)
			: base(knowledgeBase, question, questionRepository, language)
		{
			QuestionX = question;
		}
	}
}
