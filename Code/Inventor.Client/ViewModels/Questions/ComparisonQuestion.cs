namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class ComparisonQuestion : QuestionViewModel<Core.Questions.ComparisonQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamLeftValue")]
		public Core.IConcept LeftValue
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamRightValue")]
		public Core.IConcept RightValue
		{ get; set; }

		public override Core.Questions.ComparisonQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.ComparisonQuestion(LeftValue, RightValue);
		}
	}
}
