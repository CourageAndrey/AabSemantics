namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class EnumerateChildrenQuestion : IQuestion
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public IConcept Concept
		{ get; set; }
	}
}
