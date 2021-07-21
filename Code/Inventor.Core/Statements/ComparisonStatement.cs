using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Attributes;
using Inventor.Core.Base;
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

		public ComparisonStatement(IConcept leftValue, IConcept rightValue, IConcept comparisonSign)
			: base(new Func<ILanguage, String>(language => language.StatementNames.Comparison), new Func<ILanguage, String>(language => language.StatementHints.Comparison))
		{
			Update(leftValue, rightValue, comparisonSign);
		}

		public void Update(IConcept leftValue, IConcept rightValue, IConcept comparisonSign)
		{
			if (leftValue == null) throw new ArgumentNullException(nameof(leftValue));
			if (rightValue == null) throw new ArgumentNullException(nameof(rightValue));
			if (comparisonSign == null) throw new ArgumentNullException(nameof(comparisonSign));
			if (!leftValue.HasAttribute<IsValueAttribute>()) throw new ArgumentException("Left value concept has to be marked as IsValue Attribute.", nameof(leftValue));
			if (!rightValue.HasAttribute<IsValueAttribute>()) throw new ArgumentException("Right value concept has to be marked as IsValue Attribute.", nameof(rightValue));
			if (!comparisonSign.HasAttribute<IsComparisonSignAttribute>()) throw new ArgumentException("Comparison Sign concept has to be marked as IsComparisonSign Attribute.", nameof(comparisonSign));

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

		protected override IDictionary<String, INamed> GetDescriptionParameters()
		{
			return new Dictionary<String, INamed>
			{
				{ Strings.ParamLeftValue, LeftValue },
				{ Strings.ParamRightValue, RightValue },
				{ Strings.ParamComparisonSign, ComparisonSign },
			};
		}

		protected override Func<String> GetDescriptionText(ILanguageStatements language)
		{
			return () => language.Comparison;
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

		internal static readonly ICollection<Tuple<IConcept, IConcept>> Contradictions = new List<Tuple<IConcept, IConcept>>
		{
			new Tuple<IConcept, IConcept>(SystemConcepts.IsEqualTo, SystemConcepts.IsNotEqualTo),
			new Tuple<IConcept, IConcept>(SystemConcepts.IsEqualTo, SystemConcepts.IsGreaterThan),
			new Tuple<IConcept, IConcept>(SystemConcepts.IsEqualTo, SystemConcepts.IsLessThan),
			new Tuple<IConcept, IConcept>(SystemConcepts.IsGreaterThan, SystemConcepts.IsLessThan),
			new Tuple<IConcept, IConcept>(SystemConcepts.IsGreaterThan, SystemConcepts.IsLessThanOrEqualTo),
			new Tuple<IConcept, IConcept>(SystemConcepts.IsLessThan, SystemConcepts.IsGreaterThanOrEqualTo),
		};

		#endregion

		public ComparisonStatement SwapOperandsToMatchOrder(ComparisonQuestion question)
		{
			return RightValue == question.LeftValue || LeftValue == question.RightValue
				? SwapOperands()
				: this;
		}

		public ComparisonStatement SwapOperands()
		{
			return new ComparisonStatement(leftValue: RightValue, rightValue: LeftValue, ComparisonSign.Revert());
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
