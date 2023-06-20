namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsPartOfQuestion : QuestionViewModel<Semantics.Modules.Set.Questions.IsPartOfQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Parent")]
		public Semantics.IConcept Parent
		{ get; set; }

		[PropertyDescriptor(true, "Questions.Parameters.Child")]
		public Semantics.IConcept Child
		{ get; set; }

		public override Semantics.Modules.Set.Questions.IsPartOfQuestion BuildQuestionImplementation()
		{
			return new Semantics.Modules.Set.Questions.IsPartOfQuestion(Child, Parent);
		}
	}
}
