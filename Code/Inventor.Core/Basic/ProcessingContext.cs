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

		public QuestionProcessorRepository QuestionProcessorRepository
		{ get; }

		public ILanguage Language
		{ get; }

		#endregion

		public ProcessingContext(KnowledgeBase knowledgeBase, Question question, QuestionProcessorRepository questionProcessorRepository, ILanguage language)
		{
			KnowledgeBase = knowledgeBase;
			Question = question;
			QuestionProcessorRepository = questionProcessorRepository;
			Language = language;
		}

		public ProcessingContext<QuestionT> GetExplicit<QuestionT>()
			where QuestionT : Question
		{
			return new ProcessingContext<QuestionT>(KnowledgeBase, (QuestionT) Question, QuestionProcessorRepository, Language);
		}
	}

	public class ProcessingContext<QuestionT> : ProcessingContext
		where QuestionT : Question
	{
		public QuestionT QuestionX
		{ get; }

		public ProcessingContext(KnowledgeBase knowledgeBase, QuestionT question, QuestionProcessorRepository questionProcessorRepository, ILanguage language)
			: base(knowledgeBase, question, questionProcessorRepository, language)
		{
			QuestionX = question;
		}
	}
}
