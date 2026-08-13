namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the "which descendants" question.</summary>
	public class EnumerateDescendantsQuestion : QuestionViewModel<Modules.Classification.Questions.EnumerateDescendantsQuestion>
	{
		/// <summary>The concept in question.</summary>
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Classification.Questions.EnumerateDescendantsQuestion BuildQuestionImplementation()
		{
			return new Modules.Classification.Questions.EnumerateDescendantsQuestion(Concept);
		}
	}
}
