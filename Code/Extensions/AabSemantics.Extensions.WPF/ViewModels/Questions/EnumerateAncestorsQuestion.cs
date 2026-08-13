namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the "which ancestors" question.</summary>
	public class EnumerateAncestorsQuestion : QuestionViewModel<Modules.Classification.Questions.EnumerateAncestorsQuestion>
	{
		/// <summary>The concept in question.</summary>
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Classification.Questions.EnumerateAncestorsQuestion BuildQuestionImplementation()
		{
			return new Modules.Classification.Questions.EnumerateAncestorsQuestion(Concept);
		}
	}
}
