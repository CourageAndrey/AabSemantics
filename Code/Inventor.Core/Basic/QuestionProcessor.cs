namespace Inventor.Core
{
	public abstract class QuestionProcessor : IQuestionProcessor
	{
		public abstract IAnswer Process(IProcessingContext context);
	}

	public abstract class QuestionProcessor<QuestionT> : QuestionProcessor
		where QuestionT : Question
	{
		public abstract IAnswer Process(IProcessingContext<QuestionT> context);

		public override IAnswer Process(IProcessingContext context)
		{
			return Process(context.GetExplicit<QuestionT>());
		}
	}
}