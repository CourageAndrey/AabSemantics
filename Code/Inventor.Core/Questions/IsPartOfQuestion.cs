namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class IsPartOfQuestion : Question
	{
		[PropertyDescriptor(true, "QuestionNames.ParamParent")]
		public Concept Parent
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamChild")]
		public Concept Child
		{ get; set; }
	}
}
