namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateAncestorsQuestion : QuestionViewModel<Core.Questions.EnumerateAncestorsQuestion>
	{
		[PropertyDescriptor(true, "Names.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.EnumerateAncestorsQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.EnumerateAncestorsQuestion(Concept);
		}
	}
}
