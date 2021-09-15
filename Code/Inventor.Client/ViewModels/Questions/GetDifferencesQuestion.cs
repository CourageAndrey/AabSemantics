namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class GetDifferencesQuestion : QuestionViewModel<Core.Questions.GetDifferencesQuestion>
	{
		[PropertyDescriptor(true, "Set\\Questions.Parameters.Concept1")]
		public Core.IConcept Concept1
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Concept2")]
		public Core.IConcept Concept2
		{ get; set; }

		public override Core.Questions.GetDifferencesQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.GetDifferencesQuestion(Concept1, Concept2);
		}
	}
}
