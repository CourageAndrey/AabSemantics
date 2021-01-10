namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class EnumerateContainersQuestion : IQuestion
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public IConcept Concept
		{ get; set; }
	}
}
