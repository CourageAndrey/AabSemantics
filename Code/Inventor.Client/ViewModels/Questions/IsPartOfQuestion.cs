namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsPartOfQuestion : QuestionViewModel<Core.Questions.IsPartOfQuestion>
	{
		[PropertyDescriptor(true, "Names.ParamParent")]
		public Core.IConcept Parent
		{ get; set; }

		[PropertyDescriptor(true, "Names.ParamChild")]
		public Core.IConcept Child
		{ get; set; }

		public override Core.Questions.IsPartOfQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.IsPartOfQuestion(Child, Parent);
		}
	}
}
