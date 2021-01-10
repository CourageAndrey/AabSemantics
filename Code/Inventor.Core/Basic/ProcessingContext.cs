using Inventor.Core.Localization;

namespace Inventor.Core
{
	public class ProcessingContext
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

		public ProcessingContext<QuestionT> GetExplicit<QuestionT>()
			where QuestionT : Question
		{
			return new ProcessingContext<QuestionT>(KnowledgeBase, (QuestionT) Question, QuestionRepository, Language);
		}
	}

	public class ProcessingContext<QuestionT> : ProcessingContext
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
