using System;
using System.Collections.Generic;

using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;

namespace Inventor.Core.Statements
{
	public abstract class ProcessesStatement<StatementT> : Statement<StatementT>, IProcessesStatement
		where StatementT : ProcessesStatement<StatementT>
	{
		#region Properties

		public IConcept ProcessA
		{ get; private set; }

		public IConcept ProcessB
		{ get; private set; }

		#endregion

		protected ProcessesStatement(IConcept processA, IConcept processB, LocalizedString name, LocalizedString hint = null)
			: base(name, hint)
		{
			Update(processA, processB);
		}

		public void Update(IConcept processA, IConcept processB)
		{
			if (processA == null) throw new ArgumentNullException(nameof(processA));
			if (processB == null) throw new ArgumentNullException(nameof(processB));
			if (!processA.HasAttribute<IsProcessAttribute>()) throw new ArgumentException("Process A concept has to be marked as IsProcess Attribute.", nameof(processA));
			if (!processB.HasAttribute<IsProcessAttribute>()) throw new ArgumentException("Process B concept has to be marked as IsProcess Attribute.", nameof(processB));

			ProcessA = processA;
			ProcessB = processB;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return ProcessA;
			yield return ProcessB;
		}

		#region Description

		protected override IDictionary<String, INamed> GetDescriptionParameters()
		{
			return new Dictionary<String, INamed>
			{
				{ Strings.ParamProcessA, ProcessA },
				{ Strings.ParamProcessB, ProcessB },
			};
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(StatementT other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.ProcessA == ProcessA &&
						other.ProcessB == ProcessB;
			}
			else return false;
		}

		#endregion
	}
}
