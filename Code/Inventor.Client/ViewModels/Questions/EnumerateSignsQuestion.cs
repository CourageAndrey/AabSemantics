namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class EnumerateSignsQuestion : QuestionViewModel<Core.Questions.EnumerateSignsQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(false, "QuestionNames.ParamRecursive")]
		public bool Recursive
		{ get; set; }

		public override Core.Questions.EnumerateSignsQuestion BuildQuestion()
		{
			return new Core.Questions.EnumerateSignsQuestion(Concept, Recursive);
		}
	}
}
