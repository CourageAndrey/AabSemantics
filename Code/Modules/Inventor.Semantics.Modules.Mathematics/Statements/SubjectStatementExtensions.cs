using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Statements;
using Inventor.Semantics.Modules.Mathematics.Concepts;

namespace Inventor.Semantics.Modules.Mathematics.Statements
{
	public static class SubjectStatementExtensions
	{
		public static List<ComparisonStatement> IsEqualTo(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.IsEqualTo).ToList();
		}

		public static ComparisonStatement IsEqualTo(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(null, builder.Subject, other, ComparisonSigns.IsEqualTo);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ComparisonStatement> IsNotEqualTo(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.IsNotEqualTo).ToList();
		}

		public static ComparisonStatement IsNotEqualTo(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(null, builder.Subject, other, ComparisonSigns.IsNotEqualTo);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ComparisonStatement> IsGreaterThan(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.IsGreaterThan).ToList();
		}

		public static ComparisonStatement IsGreaterThan(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(null, builder.Subject, other, ComparisonSigns.IsGreaterThan);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ComparisonStatement> IsGreaterThanOrEqualTo(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.IsGreaterThanOrEqualTo).ToList();
		}

		public static ComparisonStatement IsGreaterThanOrEqualTo(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(null, builder.Subject, other, ComparisonSigns.IsGreaterThanOrEqualTo);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ComparisonStatement> IsLessThan(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.IsLessThan).ToList();
		}

		public static ComparisonStatement IsLessThan(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(null, builder.Subject, other, ComparisonSigns.IsLessThan);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ComparisonStatement> IsLessThanOrEqualTo(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.IsLessThanOrEqualTo).ToList();
		}

		public static ComparisonStatement IsLessThanOrEqualTo(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(null, builder.Subject, other, ComparisonSigns.IsLessThanOrEqualTo);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ComparisonStatement> DefineSequence(this ISemanticNetwork semanticNetwork, IEnumerable<IConcept> numbers, IConcept comparisonSign)
		{
			var comparisons = new List<ComparisonStatement>();
			IConcept leftValue = null;

			foreach (var number in numbers)
			{
				if (leftValue == null)
				{
					leftValue = number;
				}
				else
				{
					var rightValue = number;

					var comparison = new ComparisonStatement(null, leftValue, rightValue, comparisonSign);
					semanticNetwork.Statements.Add(comparison);
					comparisons.Add(comparison);

					leftValue = rightValue;
				}
			}

			return comparisons;
		}

		public static List<ComparisonStatement> DefineAscendingSequence(this ISemanticNetwork semanticNetwork, IEnumerable<IConcept> numbers)
		{
			return DefineSequence(semanticNetwork, numbers, ComparisonSigns.IsLessThan);
		}

		public static List<ComparisonStatement> DefineDescendingSequence(this ISemanticNetwork semanticNetwork, IEnumerable<IConcept> numbers)
		{
			return DefineSequence(semanticNetwork, numbers, ComparisonSigns.IsGreaterThan);
		}

		public static List<ComparisonStatement> DefineNotAscendingSequence(this ISemanticNetwork semanticNetwork, IEnumerable<IConcept> numbers)
		{
			return DefineSequence(semanticNetwork, numbers, ComparisonSigns.IsGreaterThanOrEqualTo);
		}

		public static List<ComparisonStatement> DefineNotDescendingSequence(this ISemanticNetwork semanticNetwork, IEnumerable<IConcept> numbers)
		{
			return DefineSequence(semanticNetwork, numbers, ComparisonSigns.IsLessThanOrEqualTo);
		}
	}
}
