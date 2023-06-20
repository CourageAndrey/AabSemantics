namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class GetCommonQuestion : QuestionViewModel<Semantics.Modules.Set.Questions.GetCommonQuestion>
	{
		[PropertyDescriptor(true, "Set\\Questions.Parameters.Concept1")]
		public Semantics.IConcept Concept1
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Concept2")]
		public Semantics.IConcept Concept2
		{ get; set; }

		public override Semantics.Modules.Set.Questions.GetCommonQuestion BuildQuestionImplementation()
		{
			return new Semantics.Modules.Set.Questions.GetCommonQuestion(Concept1, Concept2);
		}
	}
}
