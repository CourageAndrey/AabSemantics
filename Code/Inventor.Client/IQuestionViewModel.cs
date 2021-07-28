using System.Collections.Generic;
using System.Collections.ObjectModel;

using Inventor.Client.ViewModels.Questions;

namespace Inventor.Client
{
	public interface IQuestionViewModel
	{
		ICollection<StatementViewModel> Conditions
		{ get; }

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
		[PropertyDescriptor(true, "QuestionNames.ParamConditions")]
		public ICollection<StatementViewModel> Conditions
		{ get; } = new ObservableCollection<StatementViewModel>();

		public abstract QuestionT BuildQuestion();

		Core.IQuestion IQuestionViewModel.BuildQuestion()
		{
			return BuildQuestion();
		}
	}
}
