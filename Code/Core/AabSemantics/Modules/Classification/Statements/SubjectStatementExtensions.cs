using System.Collections.Generic;
using System.Linq;

using AabSemantics.Statements;

namespace AabSemantics.Modules.Classification.Statements
{
	public static class SubjectStatementExtensions
	{
		public static List<IsStatement> IsAncestorOf(this StatementBuilder builder, IEnumerable<IConcept> descendants)
		{
			return descendants.Select(builder.IsAncestorOf).ToList();
		}

		public static IsStatement IsAncestorOf(this StatementBuilder builder, IConcept descendant)
		{
			var statement = new IsStatement(null, builder.Subject, descendant);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<IsStatement> IsDescendantOf(this StatementBuilder builder, IEnumerable<IConcept> ancestors)
		{
			return ancestors.Select(builder.IsDescendantOf).ToList();
		}

		public static IsStatement IsDescendantOf(this StatementBuilder builder, IConcept ancestor)
		{
			var statement = new IsStatement(null, ancestor, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}
	}
}
