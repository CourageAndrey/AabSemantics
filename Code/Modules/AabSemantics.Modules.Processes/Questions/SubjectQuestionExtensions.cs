using System.Threading.Tasks;

using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Processes.Questions
{
	/// <summary>Fluent verbs for the processes module's questions.</summary>
	public static class SubjectQuestionExtensions
	{
		/// <summary>Asks how two processes relate in time.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="processA">The first process.</param>
		/// <param name="processB">The second process.</param>
		/// <returns>An answer listing the sequence signs.</returns>
		public static Task<IAnswer> WhatIsMutualSequenceOfProcessesAsync(this QuestionBuilder builder, IConcept processA, IConcept processB)
		{
			var question = new ProcessesQuestion(processA, processB, builder.Preconditions);
			return question.AskAsync(builder.SemanticNetwork.Context, null, builder.CancellationToken);
		}

		/// <summary>Blocking counterpart of <see cref="WhatIsMutualSequenceOfProcessesAsync"/>.</summary>
		/// <param name="builder">Builder carrying the network and any preconditions.</param>
		/// <param name="processA">The first process.</param>
		/// <param name="processB">The second process.</param>
		/// <returns>An answer listing the sequence signs.</returns>
		public static IAnswer WhatIsMutualSequenceOfProcesses(this QuestionBuilder builder, IConcept processA, IConcept processB)
		{
			return TaskHelper.AwaitDetached(() => builder.WhatIsMutualSequenceOfProcessesAsync(processA, processB));
		}
	}
}
