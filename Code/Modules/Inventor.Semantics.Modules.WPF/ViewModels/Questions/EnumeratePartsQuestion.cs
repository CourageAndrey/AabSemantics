namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumeratePartsQuestion : QuestionViewModel<Set.Questions.EnumeratePartsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		public override Set.Questions.EnumeratePartsQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.EnumeratePartsQuestion(Concept);
		}
	}
}
