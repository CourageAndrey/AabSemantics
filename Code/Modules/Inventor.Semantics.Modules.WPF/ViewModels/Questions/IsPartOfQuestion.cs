namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsPartOfQuestion : QuestionViewModel<Set.Questions.IsPartOfQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Parent")]
		public IConcept Parent
		{ get; set; }

		[PropertyDescriptor(true, "Questions.Parameters.Child")]
		public IConcept Child
		{ get; set; }

		public override Set.Questions.IsPartOfQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.IsPartOfQuestion(Child, Parent);
		}
	}
}
