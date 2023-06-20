using AabSemantics.Questions;

namespace AabSemantics.Modules.Processes.Questions
{
	public static class SubjectQuestionExtensions
	{
		public static IAnswer WhatIsMutualSequenceOfProcesses(this QuestionBuilder builder, IConcept processA, IConcept processB)
		{
			var question = new ProcessesQuestion(processA, processB, builder.Preconditions);
			return question.Ask(builder.SemanticNetwork.Context);
		}
	}
}
