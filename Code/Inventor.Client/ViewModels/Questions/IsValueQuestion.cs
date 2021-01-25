namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class IsValueQuestion : QuestionViewModel<Core.Questions.IsValueQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.IsValueQuestion BuildQuestion()
		{
			return new Core.Questions.IsValueQuestion(Concept);
		}
	}
}
