using System;
using System.Collections.Generic;

using Inventor.Semantics.Statements;
using Inventor.Semantics.Processes.Attributes;
using Inventor.Semantics.Processes.Concepts;
using Inventor.Semantics.Processes.Localization;
using Inventor.Semantics.Processes.Questions;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Processes.Statements
{
	public class ProcessesStatement : Statement<ProcessesStatement>
	{
		#region Properties

		public IConcept ProcessA
		{ get; private set; }

		public IConcept ProcessB
		{ get; private set; }

		public IConcept SequenceSign
		{ get; private set; }

		#endregion

		public ProcessesStatement(String id, IConcept processA, IConcept processB, IConcept sequenceSign)
			: base(
				id,
				new Func<ILanguage, String>(language => language.GetExtension<ILanguageProcessesModule>().Statements.Names.Processes),
				new Func<ILanguage, String>(language => language.GetExtension<ILanguageProcessesModule>().Statements.Hints.Processes))
		{
			Update(id, processA, processB, sequenceSign);
		}

		public void Update(String id, IConcept processA, IConcept processB, IConcept sequenceSign)
		{
			Update(id);
			ProcessA = processA.EnsureNotNull(nameof(processA)).EnsureHasAttribute<IConcept, IsProcessAttribute>(nameof(processA));
			ProcessB = processB.EnsureNotNull(nameof(processB)).EnsureHasAttribute<IConcept, IsProcessAttribute>(nameof(processB));
			SequenceSign = sequenceSign.EnsureNotNull(nameof(sequenceSign)).EnsureHasAttribute<IConcept, IsSequenceSignAttribute>(nameof(sequenceSign));
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return ProcessA;
			yield return ProcessB;
			yield return SequenceSign;
		}

		#region Description

		protected override String GetDescriptionTrueText(ILanguage language)
		{
			return language.GetExtension<ILanguageProcessesModule>().Statements.TrueFormatStrings.Processes;
		}

		protected override String GetDescriptionFalseText(ILanguage language)
		{
			return language.GetExtension<ILanguageProcessesModule>().Statements.TrueFormatStrings.Processes;
		}

		protected override String GetDescriptionQuestionText(ILanguage language)
		{
			return language.GetExtension<ILanguageProcessesModule>().Statements.TrueFormatStrings.Processes;
		}

		protected override IDictionary<String, IKnowledge> GetDescriptionParameters()
		{
			return new Dictionary<String, IKnowledge>
			{
				{ Strings.ParamProcessA, ProcessA },
				{ Strings.ParamProcessB, ProcessB },
				{ Strings.ParamSequenceSign, SequenceSign },
			};
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(ProcessesStatement other)
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

		public ProcessesStatement SwapOperandsToMatchOrder(ProcessesQuestion question)
		{
			return ProcessA == question.ProcessB || ProcessB == question.ProcessA
				? SwapOperands()
				: this;
		}

		public ProcessesStatement SwapOperands()
		{
			return new ProcessesStatement(null, processA: ProcessB, processB: ProcessA, sequenceSign: SequenceSigns.Revert(SequenceSign));
		}
	}

	public static class ProcessesStatementConsistencyExtension
	{
		public static List<Contradiction> CheckForContradictions(this IEnumerable<ProcessesStatement> statements)
		{
			var checker = new ProcessesStatementContradictionsChecker(statements);
			return checker.CheckForContradictions();
		}
	}
}
