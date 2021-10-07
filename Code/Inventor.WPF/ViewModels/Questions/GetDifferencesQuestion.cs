namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class GetDifferencesQuestion : QuestionViewModel<Set.Questions.GetDifferencesQuestion>
	{
		[PropertyDescriptor(true, "Set\\Questions.Parameters.Concept1")]
		public Core.IConcept Concept1
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Concept2")]
		public Core.IConcept Concept2
		{ get; set; }

		public override Set.Questions.GetDifferencesQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.GetDifferencesQuestion(Concept1, Concept2);
		}
	}
}
