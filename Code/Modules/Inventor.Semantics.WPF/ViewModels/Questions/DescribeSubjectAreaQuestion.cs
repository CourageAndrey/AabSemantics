namespace Inventor.Semantics.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class DescribeSubjectAreaQuestion : QuestionViewModel<Semantics.Set.Questions.DescribeSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Semantics.Set.Questions.DescribeSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Semantics.Set.Questions.DescribeSubjectAreaQuestion(Concept);
		}
	}
}
