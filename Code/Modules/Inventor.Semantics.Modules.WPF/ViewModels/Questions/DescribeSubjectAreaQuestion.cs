namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class DescribeSubjectAreaQuestion : QuestionViewModel<Semantics.Modules.Set.Questions.DescribeSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Semantics.Modules.Set.Questions.DescribeSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Semantics.Modules.Set.Questions.DescribeSubjectAreaQuestion(Concept);
		}
	}
}
