namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class CheckStatementQuestion : QuestionViewModel<Core.Questions.CheckStatementQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamStatement")]
		public StatementViewModel Statement
		{ get; set; }

		public override Core.Questions.CheckStatementQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.CheckStatementQuestion(Statement.CreateStatement());
		}
	}
}
