namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class IsSignQuestion : IQuestion
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public IConcept Concept
		{ get; set; }
	}
}
