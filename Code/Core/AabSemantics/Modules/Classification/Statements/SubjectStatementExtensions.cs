using System.Collections.Generic;
using System.Linq;

using AabSemantics.Statements;

namespace AabSemantics.Modules.Classification.Statements
{
	/// <summary>
	/// Fluent verbs declaring "is a" relations. Every one of them adds the new statement to the
	/// network immediately, in addition to returning it.
	/// </summary>
	public static class SubjectStatementExtensions
	{
		/// <summary>Declares the subject to be the ancestor of several concepts.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="descendants">Concepts that become the subject's descendants.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<IsStatement> IsAncestorOf(this StatementBuilder builder, IEnumerable<IConcept> descendants)
		{
			return descendants.Select(builder.IsAncestorOf).ToList();
		}

		/// <summary>Declares the subject to be the ancestor of one concept.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="descendant">Concept that becomes the subject's descendant.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static IsStatement IsAncestorOf(this StatementBuilder builder, IConcept descendant)
		{
			var statement = new IsStatement(null, builder.Subject, descendant);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares the subject to be a descendant of several concepts.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="ancestors">Concepts that become the subject's ancestors.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<IsStatement> IsDescendantOf(this StatementBuilder builder, IEnumerable<IConcept> ancestors)
		{
			return ancestors.Select(builder.IsDescendantOf).ToList();
		}

		/// <summary>Declares the subject to be a descendant of one concept.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="ancestor">Concept that becomes the subject's ancestor.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static IsStatement IsDescendantOf(this StatementBuilder builder, IConcept ancestor)
		{
			var statement = new IsStatement(null, ancestor, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}
	}
}
