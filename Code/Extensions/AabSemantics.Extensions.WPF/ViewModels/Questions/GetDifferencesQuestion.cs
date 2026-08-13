namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the "what is the difference" question.</summary>
	public class GetDifferencesQuestion : QuestionViewModel<Modules.Set.Questions.GetDifferencesQuestion>
	{
		/// <summary>The first compared concept.</summary>
		[PropertyDescriptor(true, "Set\\Questions.Parameters.Concept1")]
		public IConcept Concept1
		{ get; set; }

		/// <summary>The second compared concept.</summary>
		[PropertyDescriptor(true, "Set\\Questions.Parameters.Concept2")]
		public IConcept Concept2
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Set.Questions.GetDifferencesQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.GetDifferencesQuestion(Concept1, Concept2);
		}
	}
}
