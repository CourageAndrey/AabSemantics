namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class IsQuestion : IQuestion
	{
		[PropertyDescriptor(true, "QuestionNames.ParamChild")]
		public IConcept Child
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamParent")]
		public IConcept Parent
		{ get; set; }
	}
}
