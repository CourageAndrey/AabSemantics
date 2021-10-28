namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class ProcessesQuestion : QuestionViewModel<Processes.Questions.ProcessesQuestion>
	{
		[PropertyDescriptor(true, "Processes\\Questions.Parameters.ProcessA")]
		public Semantics.IConcept ProcessA
		{ get; set; }

		[PropertyDescriptor(true, "Processes\\Questions.Parameters.ProcessB")]
		public Semantics.IConcept ProcessB
		{ get; set; }

		public override Processes.Questions.ProcessesQuestion BuildQuestionImplementation()
		{
			return new Processes.Questions.ProcessesQuestion(ProcessA, ProcessB);
		}
	}
}
