using Inventor.Semantics;
using Inventor.Semantics.Questions;

namespace Inventor.Processes.Questions
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
