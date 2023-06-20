namespace Inventor.Semantics.Extensions.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class CheckStatementQuestion : QuestionViewModel<Modules.Boolean.Questions.CheckStatementQuestion>
	{
		[PropertyDescriptor(true, "Boolean\\Questions.Parameters.Statement")]
		public StatementViewModel Statement
		{ get; set; }

		public override Modules.Boolean.Questions.CheckStatementQuestion BuildQuestionImplementation()
		{
			return new Modules.Boolean.Questions.CheckStatementQuestion(Statement.CreateStatement());
		}
	}
}
