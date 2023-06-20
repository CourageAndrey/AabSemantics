namespace Inventor.Semantics.Extensions.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class ProcessesQuestion : QuestionViewModel<Modules.Processes.Questions.ProcessesQuestion>
	{
		[PropertyDescriptor(true, "Processes\\Questions.Parameters.ProcessA")]
		public IConcept ProcessA
		{ get; set; }

		[PropertyDescriptor(true, "Processes\\Questions.Parameters.ProcessB")]
		public IConcept ProcessB
		{ get; set; }

		public override Modules.Processes.Questions.ProcessesQuestion BuildQuestionImplementation()
		{
			return new Modules.Processes.Questions.ProcessesQuestion(ProcessA, ProcessB);
		}
	}
}
