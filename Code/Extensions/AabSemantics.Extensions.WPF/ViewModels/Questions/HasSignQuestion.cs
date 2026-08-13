namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the "has this sign" question.</summary>
	public class HasSignQuestion : QuestionViewModel<Modules.Set.Questions.HasSignQuestion>
	{
		/// <summary>The concept in question.</summary>
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		/// <summary>The sign concept.</summary>
		[PropertyDescriptor(true, "Set\\Questions.Parameters.Sign")]
		public IConcept Sign
		{ get; set; }

		/// <summary>Whether inherited knowledge is taken into account.</summary>
		[PropertyDescriptor(false, "Questions.Parameters.Recursive")]
		public bool Recursive
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Set.Questions.HasSignQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.HasSignQuestion(Concept, Sign, Recursive);
		}
	}
}
