namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class ProcessesQuestion : QuestionViewModel<Semantics.Processes.Questions.ProcessesQuestion>
	{
		[PropertyDescriptor(true, "Processes\\Questions.Parameters.ProcessA")]
		public Semantics.IConcept ProcessA
		{ get; set; }

		[PropertyDescriptor(true, "Processes\\Questions.Parameters.ProcessB")]
		public Semantics.IConcept ProcessB
		{ get; set; }

		public override Semantics.Processes.Questions.ProcessesQuestion BuildQuestionImplementation()
		{
			return new Semantics.Processes.Questions.ProcessesQuestion(ProcessA, ProcessB);
		}
	}
}
