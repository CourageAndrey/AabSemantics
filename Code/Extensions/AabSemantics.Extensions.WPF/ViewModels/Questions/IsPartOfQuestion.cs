namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsPartOfQuestion : QuestionViewModel<Modules.Set.Questions.IsPartOfQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Parent")]
		public IConcept Parent
		{ get; set; }

		[PropertyDescriptor(true, "Questions.Parameters.Child")]
		public IConcept Child
		{ get; set; }

		public override Modules.Set.Questions.IsPartOfQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.IsPartOfQuestion(Child, Parent);
		}
	}
}
