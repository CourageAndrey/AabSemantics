namespace Inventor.Semantics.Extensions.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class ComparisonQuestion : QuestionViewModel<Modules.Mathematics.Questions.ComparisonQuestion>
	{
		[PropertyDescriptor(true, "Mathematics\\Questions.Parameters.LeftValue")]
		public IConcept LeftValue
		{ get; set; }

		[PropertyDescriptor(true, "Mathematics\\Questions.Parameters.RightValue")]
		public IConcept RightValue
		{ get; set; }

		public override Modules.Mathematics.Questions.ComparisonQuestion BuildQuestionImplementation()
		{
			return new Modules.Mathematics.Questions.ComparisonQuestion(LeftValue, RightValue);
		}
	}
}
