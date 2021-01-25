using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class QuestionWithCondition : QuestionViewModel<Core.Questions.QuestionWithCondition>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConditions")]
		public ICollection<StatementViewModel> Conditions
		{ get; } = new ObservableCollection<StatementViewModel>();

		[PropertyDescriptor(true, "QuestionNames.ParamQuestion")]
		public IQuestionViewModel Question
		{ get; set; }

		public override Core.Questions.QuestionWithCondition BuildQuestion()
		{
			return new Core.Questions.QuestionWithCondition(Conditions.Select(condition => condition.CreateStatement()), Question.BuildQuestion());
		}
	}
}
