namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	/// <summary>Editable view over the process sequence question.</summary>
	public class ProcessesQuestion : QuestionViewModel<Modules.Processes.Questions.ProcessesQuestion>
	{
		/// <summary>The first process.</summary>
		[PropertyDescriptor(true, "Processes\\Questions.Parameters.ProcessA")]
		public IConcept ProcessA
		{ get; set; }

		/// <summary>The second process.</summary>
		[PropertyDescriptor(true, "Processes\\Questions.Parameters.ProcessB")]
		public IConcept ProcessB
		{ get; set; }

		/// <summary>Builds the question from the edited properties.</summary>
		/// <returns>The created question.</returns>
		public override Modules.Processes.Questions.ProcessesQuestion BuildQuestionImplementation()
		{
			return new Modules.Processes.Questions.ProcessesQuestion(ProcessA, ProcessB);
		}
	}
}
