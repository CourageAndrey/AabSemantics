namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class CheckStatementQuestion : QuestionViewModel<Semantics.Questions.CheckStatementQuestion>
	{
		[PropertyDescriptor(true, "Boolean\\Questions.Parameters.Statement")]
		public StatementViewModel Statement
		{ get; set; }

		public override Semantics.Questions.CheckStatementQuestion BuildQuestionImplementation()
		{
			return new Semantics.Questions.CheckStatementQuestion(Statement.CreateStatement());
		}
	}
}
