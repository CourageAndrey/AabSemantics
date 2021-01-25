namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class IsQuestion : QuestionViewModel<Core.Questions.IsQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamChild")]
		public Core.IConcept Child
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamParent")]
		public Core.IConcept Parent
		{ get; set; }

		public override Core.Questions.IsQuestion BuildQuestion()
		{
			return new Core.Questions.IsQuestion(Child, Parent);
		}
	}
}
