using System;
using System.Collections.Generic;

using Inventor.Core.Attributes;
using Inventor.Core.Concepts;
using Inventor.Core.Localization;
using Inventor.Core.Questions;

namespace Inventor.Core.Statements
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
			: base(id, new Func<ILanguage, String>(language => language.Statements.StatementNames.Comparison), new Func<ILanguage, String>(language => language.Statements.StatementHints.Comparison))
		{
			Update(id, leftValue, rightValue, comparisonSign);
		}

		public void Update(String id, IConcept leftValue, IConcept rightValue, IConcept comparisonSign)
		{
			if (leftValue == null) throw new ArgumentNullException(nameof(leftValue));
			if (rightValue == null) throw new ArgumentNullException(nameof(rightValue));
			if (comparisonSign == null) throw new ArgumentNullException(nameof(comparisonSign));
			if (!leftValue.HasAttribute<IsValueAttribute>()) throw new ArgumentException("Left value concept has to be marked as IsValue Attribute.", nameof(leftValue));
			if (!rightValue.HasAttribute<IsValueAttribute>()) throw new ArgumentException("Right value concept has to be marked as IsValue Attribute.", nameof(rightValue));
			if (!comparisonSign.HasAttribute<IsComparisonSignAttribute>()) throw new ArgumentException("Comparison Sign concept has to be marked as IsComparisonSign Attribute.", nameof(comparisonSign));

			Update(id);
			LeftValue = leftValue;
			RightValue = rightValue;
			ComparisonSign = comparisonSign;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return LeftValue;
			yield return RightValue;
			yield return ComparisonSign;
		}

		#region Description

		protected override IDictionary<String, IKnowledge> GetDescriptionParameters()
		{
			return new Dictionary<String, IKnowledge>
			{
				{ Strings.ParamLeftValue, LeftValue },
				{ Strings.ParamRightValue, RightValue },
				{ Strings.ParamComparisonSign, ComparisonSign },
			};
		}

		protected override String GetDescriptionText(ILanguageStatementsPart language)
		{
			return language.Comparison;
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(ComparisonStatement other)
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
