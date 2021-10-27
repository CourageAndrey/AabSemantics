using Inventor.Semantics.Statements;

namespace Inventor.Semantics.Modules.Classification.Statements
{
	public static class SubjectStatementExtensions
	{
		public static IsStatement IsAncestorOf(this StatementBuilder builder, IConcept descendant)
		{
			var statement = new IsStatement(null, builder.Subject, descendant);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static IsStatement IsDescendantOf(this StatementBuilder builder, IConcept ancestor)
		{
			var statement = new IsStatement(null, ancestor, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}
	}
}
