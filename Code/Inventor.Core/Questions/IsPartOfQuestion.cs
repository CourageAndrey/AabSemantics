namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class IsPartOfQuestion : Question
	{
		[PropertyDescriptor(true, "QuestionNames.ParamParent")]
		public IConcept Parent
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamChild")]
		public IConcept Child
		{ get; set; }
	}
}
