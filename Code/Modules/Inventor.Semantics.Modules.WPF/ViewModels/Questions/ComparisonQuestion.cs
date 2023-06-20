namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class ComparisonQuestion : QuestionViewModel<Semantics.Modules.Mathematics.Questions.ComparisonQuestion>
	{
		[PropertyDescriptor(true, "Mathematics\\Questions.Parameters.LeftValue")]
		public Semantics.IConcept LeftValue
		{ get; set; }

		[PropertyDescriptor(true, "Mathematics\\Questions.Parameters.RightValue")]
		public Semantics.IConcept RightValue
		{ get; set; }

		public override Semantics.Modules.Mathematics.Questions.ComparisonQuestion BuildQuestionImplementation()
		{
			return new Semantics.Modules.Mathematics.Questions.ComparisonQuestion(LeftValue, RightValue);
		}
	}
}
