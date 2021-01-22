namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class CheckStatementQuestion : IQuestion
	{
		[PropertyDescriptor(true, "QuestionNames.ParamStatement")]
		public IStatement Statement
		{ get; set; }
	}
}
