using Inventor.Core.Localization;

namespace Inventor.Core
{
	public abstract class QuestionProcessor
	{
		public abstract FormattedText Process(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, Question question, ILanguage language);
	}

	public abstract class QuestionProcessor<QuestionT> : QuestionProcessor
		where QuestionT : Question
	{
		protected abstract FormattedText ProcessImplementation(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, QuestionT question, ILanguage language);

		public override FormattedText Process(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, Question question, ILanguage language)
		{
			return ProcessImplementation(processingMechanism, knowledgeBase, (QuestionT) question, language);
		}
	}
}