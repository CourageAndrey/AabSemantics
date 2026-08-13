namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the "what is common" question.</summary>
	public class GetCommonQuestion : QuestionViewModel<Modules.Set.Questions.GetCommonQuestion>
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
		public override Modules.Set.Questions.GetCommonQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.GetCommonQuestion(Concept1, Concept2);
		}
	}
}
