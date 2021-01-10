namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class CheckStatementQuestion : IQuestion
	{
		//[PropertyDescriptor(, )]
		public IStatement Statement
		{ get; set; }
	}
}
