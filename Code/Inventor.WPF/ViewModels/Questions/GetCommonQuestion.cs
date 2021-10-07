namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class GetCommonQuestion : QuestionViewModel<Set.Questions.GetCommonQuestion>
	{
		[PropertyDescriptor(true, "Set\\Questions.Parameters.Concept1")]
		public Core.IConcept Concept1
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Concept2")]
		public Core.IConcept Concept2
		{ get; set; }

		public override Set.Questions.GetCommonQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.GetCommonQuestion(Concept1, Concept2);
		}
	}
}
