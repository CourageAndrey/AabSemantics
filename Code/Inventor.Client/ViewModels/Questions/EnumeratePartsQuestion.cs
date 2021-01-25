namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class EnumeratePartsQuestion : QuestionViewModel<Core.Questions.EnumeratePartsQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.EnumeratePartsQuestion BuildQuestion()
		{
			return new Core.Questions.EnumeratePartsQuestion(Concept);
		}
	}
}
