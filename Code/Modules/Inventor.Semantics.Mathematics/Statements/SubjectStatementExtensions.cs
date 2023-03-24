using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Statements;
using Inventor.Semantics.Mathematics.Concepts;

namespace Inventor.Semantics.Mathematics.Statements
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
	}
}
