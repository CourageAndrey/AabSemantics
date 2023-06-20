namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class ComparisonQuestion : QuestionViewModel<Mathematics.Questions.ComparisonQuestion>
	{
		[PropertyDescriptor(true, "Mathematics\\Questions.Parameters.LeftValue")]
		public IConcept LeftValue
		{ get; set; }

		[PropertyDescriptor(true, "Mathematics\\Questions.Parameters.RightValue")]
		public IConcept RightValue
		{ get; set; }

		public override Mathematics.Questions.ComparisonQuestion BuildQuestionImplementation()
		{
			return new Mathematics.Questions.ComparisonQuestion(LeftValue, RightValue);
		}
	}
}
