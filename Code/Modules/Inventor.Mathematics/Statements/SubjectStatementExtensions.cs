using Inventor.Semantics;
using Inventor.Semantics.Statements;
using Inventor.Mathematics.Concepts;

namespace Inventor.Mathematics.Statements
{
	public static class SubjectStatementExtensions
	{
		public static ComparisonStatement IsEqualTo(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(null, builder.Subject, other, ComparisonSigns.IsEqualTo);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ComparisonStatement IsNotEqualTo(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(null, builder.Subject, other, ComparisonSigns.IsNotEqualTo);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ComparisonStatement IsGreaterThan(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(null, builder.Subject, other, ComparisonSigns.IsGreaterThan);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ComparisonStatement IsGreaterThanOrEqualTo(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(null, builder.Subject, other, ComparisonSigns.IsGreaterThanOrEqualTo);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ComparisonStatement IsLessThan(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(null, builder.Subject, other, ComparisonSigns.IsLessThan);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ComparisonStatement IsLessThanOrEqualTo(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(null, builder.Subject, other, ComparisonSigns.IsLessThanOrEqualTo);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}
	}
}
