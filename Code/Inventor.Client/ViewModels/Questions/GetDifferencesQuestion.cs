namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class GetDifferencesQuestion : QuestionViewModel<Core.Questions.GetDifferencesQuestion>
	{
		[PropertyDescriptor(true, "Names.ParamConcept1")]
		public Core.IConcept Concept1
		{ get; set; }

		[PropertyDescriptor(true, "Names.ParamConcept2")]
		public Core.IConcept Concept2
		{ get; set; }

		public override Core.Questions.GetDifferencesQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.GetDifferencesQuestion(Concept1, Concept2);
		}
	}
}
