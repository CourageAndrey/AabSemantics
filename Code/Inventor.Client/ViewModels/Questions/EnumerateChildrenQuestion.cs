namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class EnumerateChildrenQuestion : QuestionViewModel<Core.Questions.EnumerateChildrenQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.EnumerateChildrenQuestion BuildQuestion()
		{
			return new Core.Questions.EnumerateChildrenQuestion(Concept);
		}
	}
}
