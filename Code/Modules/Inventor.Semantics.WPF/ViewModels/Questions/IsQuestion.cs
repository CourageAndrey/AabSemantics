namespace Inventor.Semantics.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsQuestion : QuestionViewModel<Semantics.Questions.IsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Child")]
		public Semantics.IConcept Child
		{ get; set; }

		[PropertyDescriptor(true, "Questions.Parameters.Parent")]
		public Semantics.IConcept Parent
		{ get; set; }

		public override Semantics.Questions.IsQuestion BuildQuestionImplementation()
		{
			return new Semantics.Questions.IsQuestion(Child, Parent);
		}
	}
}
