using Inventor.Core.Localization;

namespace Inventor.Core
{
	public abstract class QuestionProcessor
	{
		public abstract Answer Process(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, Question question, ILanguage language);
	}

	public abstract class QuestionProcessor<QuestionT> : QuestionProcessor
		where QuestionT : Question
	{
		protected abstract Answer ProcessImplementation(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, QuestionT question, ILanguage language);

		public override Answer Process(QuestionProcessingMechanism processingMechanism, KnowledgeBase knowledgeBase, Question question, ILanguage language)
		{
			return ProcessImplementation(processingMechanism, knowledgeBase, (QuestionT) question, language);
		}
	}
}