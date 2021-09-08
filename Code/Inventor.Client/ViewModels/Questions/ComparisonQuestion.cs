namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class ComparisonQuestion : QuestionViewModel<Core.Questions.ComparisonQuestion>
	{
		[PropertyDescriptor(true, "Names.ParamLeftValue")]
		public Core.IConcept LeftValue
		{ get; set; }

		[PropertyDescriptor(true, "Names.ParamRightValue")]
		public Core.IConcept RightValue
		{ get; set; }

		public override Core.Questions.ComparisonQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.ComparisonQuestion(LeftValue, RightValue);
		}
	}
}
