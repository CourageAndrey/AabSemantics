using System;
using System.Collections.Generic;

using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;

namespace Inventor.Core.Statements
{
	public abstract class ComparisonStatement<StatementT> : Statement<StatementT>, IComparisonStatement
		where StatementT : ComparisonStatement<StatementT>
	{
		#region Properties

		public IConcept LeftValue
		{ get; private set; }

		public IConcept RightValue
		{ get; private set; }

		#endregion

		protected ComparisonStatement(IConcept leftValue, IConcept rightValue, LocalizedString name, LocalizedString hint = null)
			: base(name, hint)
		{
			Update(leftValue, rightValue);
		}

		public void Update(IConcept leftValue, IConcept rightValue)
		{
			if (leftValue == null) throw new ArgumentNullException(nameof(leftValue));
			if (rightValue == null) throw new ArgumentNullException(nameof(rightValue));
			if (!leftValue.HasAttribute<IsValueAttribute>()) throw new ArgumentException("Left value concept has to be marked as IsValue Attribute.", nameof(leftValue));
			if (!rightValue.HasAttribute<IsValueAttribute>()) throw new ArgumentException("Right value concept has to be marked as IsValue Attribute.", nameof(rightValue));

			LeftValue = leftValue;
			RightValue = rightValue;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return LeftValue;
			yield return RightValue;
		}

		#region Description

		protected override IDictionary<String, INamed> GetDescriptionParameters()
		{
			return new Dictionary<String, INamed>
			{
				{ Strings.ParamLeftValue, LeftValue },
				{ Strings.ParamRightValue, RightValue },
			};
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(StatementT other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.LeftValue == LeftValue &&
						other.RightValue == RightValue;
			}
			else return false;
		}

		#endregion
	}
}
