namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the "which containers" question.</summary>
	public class EnumerateContainersQuestion : QuestionViewModel<Modules.Set.Questions.EnumerateContainersQuestion>
	{
		/// <summary>The concept in question.</summary>
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Set.Questions.EnumerateContainersQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.EnumerateContainersQuestion(Concept);
		}
	}
}
