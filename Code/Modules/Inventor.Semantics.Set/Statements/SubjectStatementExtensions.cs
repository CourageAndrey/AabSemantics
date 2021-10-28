using Inventor.Semantics;
using Inventor.Semantics.Statements;

namespace Inventor.Semantics.Set.Statements
{
	public static class SubjectStatementExtensions
	{
		public static HasPartStatement IsPartOf(this StatementBuilder builder, IConcept whole)
		{
			var statement = new HasPartStatement(null, whole, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static HasPartStatement HasPart(this StatementBuilder builder, IConcept part)
		{
			var statement = new HasPartStatement(null, builder.Subject, part);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static GroupStatement BelongsToSubjectArea(this StatementBuilder builder, IConcept subjectArea)
		{
			var statement = new GroupStatement(null, subjectArea, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static GroupStatement IsSubjectAreaOf(this StatementBuilder builder, IConcept concept)
		{
			var statement = new GroupStatement(null, builder.Subject, concept);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static HasSignStatement HasSign(this StatementBuilder builder, IConcept sign)
		{
			var statement = new HasSignStatement(null, builder.Subject, sign);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static HasSignStatement IsSignOf(this StatementBuilder builder, IConcept concept)
		{
			var statement = new HasSignStatement(null, concept, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static SignValueStatement HasSignValue(this StatementBuilder builder, IConcept sign, IConcept value)
		{
			var statement = new SignValueStatement(null, builder.Subject, sign, value);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static SignValueStatement IsSignValue(this StatementBuilder builder, IConcept concept, IConcept sign)
		{
			var statement = new SignValueStatement(null, concept, sign, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}
	}
}
