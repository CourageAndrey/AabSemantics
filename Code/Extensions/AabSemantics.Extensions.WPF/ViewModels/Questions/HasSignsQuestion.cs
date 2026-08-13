namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the "has any signs" question.</summary>
	public class HasSignsQuestion : QuestionViewModel<Modules.Set.Questions.HasSignsQuestion>
	{
		/// <summary>The concept in question.</summary>
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		/// <summary>Whether inherited knowledge is taken into account.</summary>
		[PropertyDescriptor(false, "Questions.Parameters.Recursive")]
		public bool Recursive
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Set.Questions.HasSignsQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.HasSignsQuestion(Concept, Recursive);
		}
	}
}
