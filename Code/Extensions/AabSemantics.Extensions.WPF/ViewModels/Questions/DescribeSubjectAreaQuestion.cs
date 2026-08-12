namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	public class DescribeSubjectAreaQuestion : QuestionViewModel<Modules.Set.Questions.DescribeSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		public override Modules.Set.Questions.DescribeSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.DescribeSubjectAreaQuestion(Concept);
		}
	}
}
