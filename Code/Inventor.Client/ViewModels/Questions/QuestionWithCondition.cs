using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class QuestionWithCondition : QuestionViewModel<Core.Questions.QuestionWithCondition>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConditions")]
		public ICollection<Core.IStatement> Conditions
		{ get; } = new ObservableCollection<Core.IStatement>();

		[PropertyDescriptor(true, "QuestionNames.ParamQuestion")]
		public IQuestionViewModel Question
		{ get; set; }

		public override Core.Questions.QuestionWithCondition BuildQuestion()
		{
			return new Core.Questions.QuestionWithCondition(Conditions, Question.BuildQuestion());
		}
	}
}
