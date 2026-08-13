namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the subject area contents question.</summary>
	public class DescribeSubjectAreaQuestion : QuestionViewModel<Modules.Set.Questions.DescribeSubjectAreaQuestion>
	{
		/// <summary>The concept in question.</summary>
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Set.Questions.DescribeSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.DescribeSubjectAreaQuestion(Concept);
		}
	}
}
