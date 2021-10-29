namespace Inventor.Semantics.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsPartOfQuestion : QuestionViewModel<Semantics.Set.Questions.IsPartOfQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Parent")]
		public Semantics.IConcept Parent
		{ get; set; }

		[PropertyDescriptor(true, "Questions.Parameters.Child")]
		public Semantics.IConcept Child
		{ get; set; }

		public override Semantics.Set.Questions.IsPartOfQuestion BuildQuestionImplementation()
		{
			return new Semantics.Set.Questions.IsPartOfQuestion(Child, Parent);
		}
	}
}
