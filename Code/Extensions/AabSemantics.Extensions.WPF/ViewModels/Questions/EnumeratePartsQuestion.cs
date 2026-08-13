namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the "which parts" question.</summary>
	public class EnumeratePartsQuestion : QuestionViewModel<Modules.Set.Questions.EnumeratePartsQuestion>
	{
		/// <summary>The concept in question.</summary>
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Set.Questions.EnumeratePartsQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.EnumeratePartsQuestion(Concept);
		}
	}
}
