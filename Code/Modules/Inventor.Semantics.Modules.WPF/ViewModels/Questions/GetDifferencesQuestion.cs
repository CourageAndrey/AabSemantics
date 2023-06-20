namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class GetDifferencesQuestion : QuestionViewModel<Semantics.Modules.Set.Questions.GetDifferencesQuestion>
	{
		[PropertyDescriptor(true, "Set\\Questions.Parameters.Concept1")]
		public Semantics.IConcept Concept1
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Concept2")]
		public Semantics.IConcept Concept2
		{ get; set; }

		public override Semantics.Modules.Set.Questions.GetDifferencesQuestion BuildQuestionImplementation()
		{
			return new Semantics.Modules.Set.Questions.GetDifferencesQuestion(Concept1, Concept2);
		}
	}
}
