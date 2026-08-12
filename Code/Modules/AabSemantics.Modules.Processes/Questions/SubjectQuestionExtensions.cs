using System.Threading.Tasks;

using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Processes.Questions
{
	public static class SubjectQuestionExtensions
	{
		public static async Task<IAnswer> WhatIsMutualSequenceOfProcessesAsync(this QuestionBuilder builder, IConcept processA, IConcept processB)
		{
			var question = new ProcessesQuestion(processA, processB, builder.Preconditions);
			return await question.AskAsync(builder.SemanticNetwork.Context).ConfigureAwait(false);
		}

		public static IAnswer WhatIsMutualSequenceOfProcesses(this QuestionBuilder builder, IConcept processA, IConcept processB)
		{
			return TaskHelper.AwaitDetached(() => builder.WhatIsMutualSequenceOfProcessesAsync(processA, processB));
		}
	}
}
