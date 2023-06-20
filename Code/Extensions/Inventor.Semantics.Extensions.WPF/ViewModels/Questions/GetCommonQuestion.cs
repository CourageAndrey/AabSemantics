namespace Inventor.Semantics.Extensions.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class GetCommonQuestion : QuestionViewModel<Modules.Set.Questions.GetCommonQuestion>
	{
		[PropertyDescriptor(true, "Set\\Questions.Parameters.Concept1")]
		public IConcept Concept1
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Concept2")]
		public IConcept Concept2
		{ get; set; }

		public override Modules.Set.Questions.GetCommonQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.GetCommonQuestion(Concept1, Concept2);
		}
	}
}
