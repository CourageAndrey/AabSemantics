namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the "is a subject area" question.</summary>
	public class IsSubjectAreaQuestion : QuestionViewModel<Modules.Set.Questions.IsSubjectAreaQuestion>
	{
		/// <summary>The concept in question.</summary>
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		/// <summary>The subject area concept.</summary>
		[PropertyDescriptor(true, "Set\\Questions.Parameters.Area")]
		public IConcept Area
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Set.Questions.IsSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.IsSubjectAreaQuestion(Concept, Area);
		}
	}
}
