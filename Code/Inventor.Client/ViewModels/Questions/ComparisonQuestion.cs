namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class ComparisonQuestion : QuestionViewModel<Mathematics.Questions.ComparisonQuestion>
	{
		[PropertyDescriptor(true, "Mathematics\\Questions.Parameters.LeftValue")]
		public Core.IConcept LeftValue
		{ get; set; }

		[PropertyDescriptor(true, "Mathematics\\Questions.Parameters.RightValue")]
		public Core.IConcept RightValue
		{ get; set; }

		public override Mathematics.Questions.ComparisonQuestion BuildQuestionImplementation()
		{
			return new Mathematics.Questions.ComparisonQuestion(LeftValue, RightValue);
		}
	}
}
