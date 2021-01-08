using Inventor.Core.Localization;

namespace Inventor.Core
{
	public abstract class QuestionProcessor
	{
		public abstract FormattedText Process(KnowledgeBase knowledgeBase, Question question, ILanguage language);
	}

	public abstract class QuestionProcessor<QuestionT> : QuestionProcessor
		where QuestionT : Question
	{
		protected abstract FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, QuestionT question, ILanguage language);

		public override FormattedText Process(KnowledgeBase knowledgeBase, Question question, ILanguage language)
		{
			return ProcessImplementation(knowledgeBase, (QuestionT) question, language);
		}
	}
}