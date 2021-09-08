namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateSignsQuestion : QuestionViewModel<Core.Questions.EnumerateSignsQuestion>
	{
		[PropertyDescriptor(true, "Names.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(false, "Names.ParamRecursive")]
		public bool Recursive
		{ get; set; }

		public override Core.Questions.EnumerateSignsQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.EnumerateSignsQuestion(Concept, Recursive);
		}
	}
}
