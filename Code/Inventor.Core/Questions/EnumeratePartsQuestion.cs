namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class EnumeratePartsQuestion : IQuestion
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public IConcept Concept
		{ get; set; }
	}
}
