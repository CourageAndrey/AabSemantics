namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsQuestion : QuestionViewModel<Classification.Questions.IsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Child")]
		public IConcept Child
		{ get; set; }

		[PropertyDescriptor(true, "Questions.Parameters.Parent")]
		public IConcept Parent
		{ get; set; }

		public override Classification.Questions.IsQuestion BuildQuestionImplementation()
		{
			return new Classification.Questions.IsQuestion(Child, Parent);
		}
	}
}
