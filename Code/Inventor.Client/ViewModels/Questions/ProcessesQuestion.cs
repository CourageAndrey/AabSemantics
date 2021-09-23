﻿namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class ProcessesQuestion : QuestionViewModel<Processes.Questions.ProcessesQuestion>
	{
		[PropertyDescriptor(true, "Processes\\Questions.Parameters.ProcessA")]
		public Core.IConcept ProcessA
		{ get; set; }

		[PropertyDescriptor(true, "Processes\\Questions.Parameters.ProcessB")]
		public Core.IConcept ProcessB
		{ get; set; }

		public override Processes.Questions.ProcessesQuestion BuildQuestionImplementation()
		{
			return new Processes.Questions.ProcessesQuestion(ProcessA, ProcessB);
		}
	}
}
