namespace Inventor.Semantics.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class GetCommonQuestion : QuestionViewModel<Semantics.Set.Questions.GetCommonQuestion>
	{
		[PropertyDescriptor(true, "Set\\Questions.Parameters.Concept1")]
		public Semantics.IConcept Concept1
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Concept2")]
		public Semantics.IConcept Concept2
		{ get; set; }

		public override Semantics.Set.Questions.GetCommonQuestion BuildQuestionImplementation()
		{
			return new Semantics.Set.Questions.GetCommonQuestion(Concept1, Concept2);
		}
	}
}
