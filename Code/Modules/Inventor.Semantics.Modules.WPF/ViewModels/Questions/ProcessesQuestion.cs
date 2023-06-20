namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class ProcessesQuestion : QuestionViewModel<Semantics.Modules.Processes.Questions.ProcessesQuestion>
	{
		[PropertyDescriptor(true, "Processes\\Questions.Parameters.ProcessA")]
		public Semantics.IConcept ProcessA
		{ get; set; }

		[PropertyDescriptor(true, "Processes\\Questions.Parameters.ProcessB")]
		public Semantics.IConcept ProcessB
		{ get; set; }

		public override Semantics.Modules.Processes.Questions.ProcessesQuestion BuildQuestionImplementation()
		{
			return new Semantics.Modules.Processes.Questions.ProcessesQuestion(ProcessA, ProcessB);
		}
	}
}
