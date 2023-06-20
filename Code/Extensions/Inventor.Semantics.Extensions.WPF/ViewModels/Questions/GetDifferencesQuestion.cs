namespace Inventor.Semantics.Extensions.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class GetDifferencesQuestion : QuestionViewModel<Modules.Set.Questions.GetDifferencesQuestion>
	{
		[PropertyDescriptor(true, "Set\\Questions.Parameters.Concept1")]
		public IConcept Concept1
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Concept2")]
		public IConcept Concept2
		{ get; set; }

		public override Modules.Set.Questions.GetDifferencesQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.GetDifferencesQuestion(Concept1, Concept2);
		}
	}
}
