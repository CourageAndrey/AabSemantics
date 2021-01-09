namespace Inventor.Core
{
	public abstract class QuestionProcessor
	{
		public abstract Answer Process(ProcessingContext context);
	}

	public abstract class QuestionProcessor<QuestionT> : QuestionProcessor
		where QuestionT : Question
	{
		public abstract Answer Process(ProcessingContext<QuestionT> context);

		public override Answer Process(ProcessingContext context)
		{
			return Process(context.GetExplicit<QuestionT>());
		}
	}
}