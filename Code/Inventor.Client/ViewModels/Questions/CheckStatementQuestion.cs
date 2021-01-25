namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class CheckStatementQuestion : QuestionViewModel<Core.Questions.CheckStatementQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamStatement")]
		public Core.IStatement Statement
		{ get; set; }

		public override Core.Questions.CheckStatementQuestion BuildQuestion()
		{
			return new Core.Questions.CheckStatementQuestion(Statement);
		}
	}
}
