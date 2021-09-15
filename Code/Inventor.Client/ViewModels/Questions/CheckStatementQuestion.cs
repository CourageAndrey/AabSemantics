namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class CheckStatementQuestion : QuestionViewModel<Core.Questions.CheckStatementQuestion>
	{
		[PropertyDescriptor(true, "Boolean\\Questions.Parameters.Statement")]
		public StatementViewModel Statement
		{ get; set; }

		public override Core.Questions.CheckStatementQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.CheckStatementQuestion(Statement.CreateStatement());
		}
	}
}
