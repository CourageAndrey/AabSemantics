namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class IsSignQuestion : QuestionViewModel<Core.Questions.IsSignQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.IsSignQuestion BuildQuestion()
		{
			return new Core.Questions.IsSignQuestion(Concept);
		}
	}
}
