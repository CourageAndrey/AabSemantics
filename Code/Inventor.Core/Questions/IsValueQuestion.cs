namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class IsValueQuestion : IQuestion
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public IConcept Concept
		{ get; set; }
	}
}
