namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class ComparisonQuestion : QuestionViewModel<Core.Questions.ComparisonQuestion>
	{
		[PropertyDescriptor(true, "Mathematics\\Questions.Parameters.LeftValue")]
		public Core.IConcept LeftValue
		{ get; set; }

		[PropertyDescriptor(true, "Mathematics\\Questions.Parameters.RightValue")]
		public Core.IConcept RightValue
		{ get; set; }

		public override Core.Questions.ComparisonQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.ComparisonQuestion(LeftValue, RightValue);
		}
	}
}
