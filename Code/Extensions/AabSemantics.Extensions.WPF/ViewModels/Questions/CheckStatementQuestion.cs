namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the "is this statement true" question.</summary>
	public class CheckStatementQuestion : QuestionViewModel<Modules.Boolean.Questions.CheckStatementQuestion>
	{
		/// <summary>The statement being edited.</summary>
		/// <summary>The statement being edited.</summary>
		[PropertyDescriptor(true, "Boolean\\Questions.Parameters.Statement")]
		public StatementViewModel Statement
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Boolean.Questions.CheckStatementQuestion BuildQuestionImplementation()
		{
			return new Modules.Boolean.Questions.CheckStatementQuestion(Statement.CreateStatement());
		}
	}
}
