namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class IsQuestion : IQuestion
	{
		[PropertyDescriptor(true, "QuestionNames.ParamChild")]
		public IConcept ChildConcept
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamParent")]
		public IConcept ParentConcept
		{ get; set; }
	}
}
