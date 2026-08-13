namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the "is a value" question.</summary>
	public class IsValueQuestion : QuestionViewModel<Modules.Set.Questions.IsValueQuestion>
	{
		/// <summary>The concept in question.</summary>
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Set.Questions.IsValueQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.IsValueQuestion(Concept);
		}
	}
}
