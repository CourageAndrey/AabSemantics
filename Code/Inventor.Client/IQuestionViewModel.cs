namespace Inventor.Client
{
	public interface IQuestionViewModel
	{
		Core.IQuestion BuildQuestion();
	}

	public interface IQuestionViewModel<out QuestionT> : IQuestionViewModel
		where QuestionT : Core.IQuestion
	{
		new QuestionT BuildQuestion();
	}

	public abstract class QuestionViewModel<QuestionT> : IQuestionViewModel<QuestionT>
		where QuestionT : Core.IQuestion
	{
		public abstract QuestionT BuildQuestion();

		Core.IQuestion IQuestionViewModel.BuildQuestion()
		{
			return BuildQuestion();
		}
	}
}
