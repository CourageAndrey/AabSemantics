namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class HasSignQuestion : QuestionViewModel<Core.Questions.HasSignQuestion>
	{
		[PropertyDescriptor(true, "Names.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "Names.ParamSign")]
		public Core.IConcept Sign
		{ get; set; }

		[PropertyDescriptor(false, "Names.ParamRecursive")]
		public bool Recursive
		{ get; set; }

		public override Core.Questions.HasSignQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.HasSignQuestion(Concept, Sign, Recursive);
		}
	}
}
