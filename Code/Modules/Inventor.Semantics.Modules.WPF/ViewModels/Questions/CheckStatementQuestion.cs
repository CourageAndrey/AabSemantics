namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class CheckStatementQuestion : QuestionViewModel<Boolean.Questions.CheckStatementQuestion>
	{
		[PropertyDescriptor(true, "Boolean\\Questions.Parameters.Statement")]
		public StatementViewModel Statement
		{ get; set; }

		public override Boolean.Questions.CheckStatementQuestion BuildQuestionImplementation()
		{
			return new Boolean.Questions.CheckStatementQuestion(Statement.CreateStatement());
		}
	}
}
