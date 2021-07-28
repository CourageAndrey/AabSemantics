namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class HasSignsQuestion : QuestionViewModel<Core.Questions.HasSignsQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(false, "QuestionNames.ParamRecursive")]
		public bool Recursive
		{ get; set; }

		public override Core.Questions.HasSignsQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.HasSignsQuestion(Concept, Recursive);
		}
	}
}
