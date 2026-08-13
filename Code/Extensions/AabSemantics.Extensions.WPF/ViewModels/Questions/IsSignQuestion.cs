namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the "is a sign" question.</summary>
	public class IsSignQuestion : QuestionViewModel<Modules.Set.Questions.IsSignQuestion>
	{
		/// <summary>The concept in question.</summary>
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Set.Questions.IsSignQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.IsSignQuestion(Concept);
		}
	}
}
