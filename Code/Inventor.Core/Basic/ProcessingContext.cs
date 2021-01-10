namespace Inventor.Core
{
	public class ProcessingContext : IProcessingContext
	{
		#region Properties

		public KnowledgeBase KnowledgeBase
		{ get; }

		public Question Question
		{ get; }

		public QuestionRepository QuestionRepository
		{ get; }

		public ILanguage Language
		{ get; }

		#endregion

		public ProcessingContext(KnowledgeBase knowledgeBase, Question question, QuestionRepository questionRepository, ILanguage language)
		{
			KnowledgeBase = knowledgeBase;
			Question = question;
			QuestionRepository = questionRepository;
			Language = language;
		}
	}

	public class ProcessingContext<QuestionT> : ProcessingContext, IProcessingContext<QuestionT>
		where QuestionT : Question
	{
		public QuestionT QuestionX
		{ get; }

		public ProcessingContext(KnowledgeBase knowledgeBase, QuestionT question, QuestionRepository questionRepository, ILanguage language)
			: base(knowledgeBase, question, questionRepository, language)
		{
			QuestionX = question;
		}
	}
}
