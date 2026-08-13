namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the "which subject areas" question.</summary>
	public class FindSubjectAreaQuestion : QuestionViewModel<Modules.Set.Questions.FindSubjectAreaQuestion>
	{
		/// <summary>The concept in question.</summary>
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Set.Questions.FindSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.FindSubjectAreaQuestion(Concept);
		}
	}
}
