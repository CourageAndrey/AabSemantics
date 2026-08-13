namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the sign value question.</summary>
	public class SignValueQuestion : QuestionViewModel<Modules.Set.Questions.SignValueQuestion>
	{
		/// <summary>The concept in question.</summary>
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		/// <summary>The sign concept.</summary>
		[PropertyDescriptor(true, "Set\\Questions.Parameters.Sign")]
		public IConcept Sign
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Set.Questions.SignValueQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.SignValueQuestion(Concept, Sign);
		}
	}
}
