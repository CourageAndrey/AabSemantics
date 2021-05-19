namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class ProcessesQuestion : QuestionViewModel<Core.Questions.ProcessesQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamProcessA")]
		public Core.IConcept ProcessA
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamProcessB")]
		public Core.IConcept ProcessB
		{ get; set; }

		public override Core.Questions.ProcessesQuestion BuildQuestion()
		{
			return new Core.Questions.ProcessesQuestion(ProcessA, ProcessB);
		}
	}
}
