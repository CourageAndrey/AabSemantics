namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class HasSignsQuestion : QuestionViewModel<Core.Questions.HasSignsQuestion>
	{
		[PropertyDescriptor(true, "Names.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(false, "Names.ParamRecursive")]
		public bool Recursive
		{ get; set; }

		public override Core.Questions.HasSignsQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.HasSignsQuestion(Concept, Recursive);
		}
	}
}
