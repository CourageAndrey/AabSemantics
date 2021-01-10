namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class CheckStatementQuestion : Question
	{
		//[PropertyDescriptor(, )]
		public IStatement Statement
		{ get; set; }
	}
}
