using System;
using System.Collections.Generic;

using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Mathematics.Attributes;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Mathematics.Localization;
using AabSemantics.Modules.Mathematics.Questions;
using AabSemantics.Statements;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Mathematics.Statements
{
	public class ComparisonStatement : Statement<ComparisonStatement>
	{
		#region Properties

		public IConcept LeftValue
		{ get; private set; }

		public IConcept RightValue
		{ get; private set; }

		public IConcept ComparisonSign
		{ get; private set; }

		#endregion

		public ComparisonStatement(String id, IConcept leftValue, IConcept rightValue, IConcept comparisonSign)
			: base(
				id,
				new Func<ILanguage, String>(language => language.GetExtension<ILanguageMathematicsModule>().Statements.Names.Comparison),
				new Func<ILanguage, String>(language => language.GetExtension<ILanguageMathematicsModule>().Statements.Hints.Comparison))
		{
			Update(id, leftValue, rightValue, comparisonSign);
		}

		public void Update(String id, IConcept leftValue, IConcept rightValue, IConcept comparisonSign)
		{
			Update(id);
			LeftValue = leftValue.EnsureNotNull(nameof(leftValue)).EnsureHasAttribute<IConcept, IsValueAttribute>(nameof(leftValue));
			RightValue = rightValue.EnsureNotNull(nameof(rightValue)).EnsureHasAttribute<IConcept, IsValueAttribute>(nameof(rightValue));
			ComparisonSign = comparisonSign.EnsureNotNull(nameof(comparisonSign)).EnsureHasAttribute<IConcept, IsComparisonSignAttribute>(nameof(comparisonSign));
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return LeftValue;
			yield return RightValue;
			yield return ComparisonSign;
		}

		#region Consistency checking

		public override System.Boolean Equals(ComparisonStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.LeftValue == LeftValue &&
						other.RightValue == RightValue &&
						other.ComparisonSign == ComparisonSign;
			}
			else return false;
		}

		#endregion

		public ComparisonStatement SwapOperandsToMatchOrder(ComparisonQuestion question)
		{
			return RightValue == question.LeftValue || LeftValue == question.RightValue
				? SwapOperands()
				: this;
		}

		public ComparisonStatement SwapOperands()
		{
			return new ComparisonStatement(null, leftValue: RightValue, rightValue: LeftValue, comparisonSign: ComparisonSigns.Revert(ComparisonSign));
		}
	}

	public static class ComparisonStatementConsistencyExtension
	{
		public static List<Contradiction> CheckForContradictions(this IEnumerable<ComparisonStatement> statements)
		{
			var checker = new ComparisonStatementContradictionsChecker(statements);
			return checker.CheckForContradictions();
		}
	}
}
