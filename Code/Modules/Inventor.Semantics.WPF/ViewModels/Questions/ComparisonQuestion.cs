namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class ComparisonQuestion : QuestionViewModel<Semantics.Mathematics.Questions.ComparisonQuestion>
	{
		[PropertyDescriptor(true, "Mathematics\\Questions.Parameters.LeftValue")]
		public Semantics.IConcept LeftValue
		{ get; set; }

		[PropertyDescriptor(true, "Mathematics\\Questions.Parameters.RightValue")]
		public Semantics.IConcept RightValue
		{ get; set; }

		public override Semantics.Mathematics.Questions.ComparisonQuestion BuildQuestionImplementation()
		{
			return new Semantics.Mathematics.Questions.ComparisonQuestion(LeftValue, RightValue);
		}
	}
}
