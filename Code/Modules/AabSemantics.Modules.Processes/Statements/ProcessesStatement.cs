using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Concepts;
using AabSemantics.Modules.Processes.Localization;
using AabSemantics.Modules.Processes.Questions;
using AabSemantics.Statements;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Processes.Statements
{
	/// <summary>
	/// States how two processes relate in time, e.g. "A starts after B finished". The operand
	/// order matters, so the same fact can be written two ways; see <see cref="SwapOperands"/>.
	/// </summary>
	public class ProcessesStatement : Statement<ProcessesStatement>
	{
		#region Properties

		/// <summary>The first process; must carry the "is a process" attribute.</summary>
		public IConcept ProcessA
		{ get; private set; }

		/// <summary>The second process; must carry the "is a process" attribute.</summary>
		public IConcept ProcessB
		{ get; private set; }

		/// <summary>The temporal relation between them; must carry the "is a sequence sign" attribute.</summary>
		public IConcept SequenceSign
		{ get; private set; }

		#endregion

		/// <summary>Creates a process sequence statement.</summary>
		/// <param name="id">Identifier; a GUID is generated when null or empty.</param>
		/// <param name="processA">The first process.</param>
		/// <param name="processB">The second process.</param>
		/// <param name="sequenceSign">The temporal relation between them.</param>
		/// <exception cref="ArgumentNullException">Any concept is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">A concept lacks the attribute its role requires.</exception>
		public ProcessesStatement(String id, IConcept processA, IConcept processB, IConcept sequenceSign)
			: base(
				id,
				new Func<ILanguage, String>(language => language.GetStatementsExtension<ILanguageProcessesModule, ILanguageStatements>().Names.Processes),
				new Func<ILanguage, String>(language => language.GetStatementsExtension<ILanguageProcessesModule, ILanguageStatements>().Hints.Processes))
		{
			Update(id, processA, processB, sequenceSign);
		}

		/// <summary>Reassigns the identifier and all three concepts, re-checking their attributes.</summary>
		/// <param name="id">New identifier; a GUID is generated when null or empty.</param>
		/// <param name="processA">The first process.</param>
		/// <param name="processB">The second process.</param>
		/// <param name="sequenceSign">The temporal relation between them.</param>
		/// <exception cref="ArgumentNullException">Any concept is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">A concept lacks the attribute its role requires.</exception>
		public void Update(String id, IConcept processA, IConcept processB, IConcept sequenceSign)
		{
			Update(id);
			ProcessA = processA.EnsureNotNull(nameof(processA)).EnsureHasAttribute<IConcept, IsProcessAttribute>(nameof(processA));
			ProcessB = processB.EnsureNotNull(nameof(processB)).EnsureHasAttribute<IConcept, IsProcessAttribute>(nameof(processB));
			SequenceSign = sequenceSign.EnsureNotNull(nameof(sequenceSign)).EnsureHasAttribute<IConcept, IsSequenceSignAttribute>(nameof(sequenceSign));
		}

		/// <summary>Returns both processes and the sign.</summary>
		/// <returns>The first process, the second process and the sequence sign.</returns>
		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return ProcessA;
			yield return ProcessB;
			yield return SequenceSign;
		}

		#region Consistency checking

		/// <summary>
		/// Compares all three concepts by reference. A statement and its swapped equivalent are
		/// <em>not</em> equal, even though they assert the same thing.
		/// </summary>
		/// <param name="other">Statement to compare with; may be <c>null</c>.</param>
		/// <returns><c>true</c> if both hold the same processes in the same order with the same sign.</returns>
		public override System.Boolean Equals(ProcessesStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.ProcessA == ProcessA &&
						other.ProcessB == ProcessB &&
						other.SequenceSign == SequenceSign;
			}
			else return false;
		}

		#endregion

		/// <summary>Returns this statement with its processes ordered the way a question asks about them.</summary>
		/// <param name="question">Question whose operand order should be matched.</param>
		/// <returns>A swapped copy when the orders differ; otherwise this statement itself.</returns>
		public ProcessesStatement SwapOperandsToMatchOrder(ProcessesQuestion question)
		{
			return ProcessA == question.ProcessB || ProcessB == question.ProcessA
				? SwapOperands()
				: this;
		}

		/// <summary>
		/// Returns the equivalent statement with the processes exchanged and the sign reverted.
		/// The copy gets a fresh identifier and is not added to any network.
		/// </summary>
		/// <returns>A new, equivalent statement.</returns>
		public ProcessesStatement SwapOperands()
		{
			return new ProcessesStatement(null, processA: ProcessB, processB: ProcessA, sequenceSign: SequenceSigns.Revert(SequenceSign));
		}
	}

	/// <summary>Consistency checking over a set of process sequence statements.</summary>
	public static class ProcessesStatementConsistencyExtension
	{
		/// <summary>Infers everything derivable from the statements and reports the contradictions found.</summary>
		/// <param name="statements">Statements to analyse.</param>
		/// <param name="cancellationToken">Cancels the analysis.</param>
		/// <returns>One entry per contradicting pair of processes; empty when the set is consistent.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static async Task<List<Contradiction>> CheckForContradictionsAsync(this IEnumerable<ProcessesStatement> statements, CancellationToken cancellationToken = default)
		{
			var checker = new ProcessesStatementContradictionsChecker(statements);
			return await checker.CheckForContradictionsAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <summary>Blocking counterpart of <see cref="CheckForContradictionsAsync"/>.</summary>
		/// <param name="statements">Statements to analyse.</param>
		/// <param name="cancellationToken">Cancels the analysis.</param>
		/// <returns>One entry per contradicting pair of processes.</returns>
		public static List<Contradiction> CheckForContradictions(this IEnumerable<ProcessesStatement> statements, CancellationToken cancellationToken = default)
		{
			return TaskHelper.AwaitDetached(() => CheckForContradictionsAsync(statements, cancellationToken));
		}
	}
}
