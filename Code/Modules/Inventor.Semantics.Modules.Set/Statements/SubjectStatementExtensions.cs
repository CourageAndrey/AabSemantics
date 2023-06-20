using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Statements;

namespace Inventor.Semantics.Set.Statements
{
	public static class SubjectStatementExtensions
	{
		public static List<HasPartStatement> IsPartOf(this StatementBuilder builder, IEnumerable<IConcept> wholes)
		{
			return wholes.Select(builder.IsPartOf).ToList();
		}

		public static HasPartStatement IsPartOf(this StatementBuilder builder, IConcept whole)
		{
			var statement = new HasPartStatement(null, whole, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<HasPartStatement> HasParts(this StatementBuilder builder, IEnumerable<IConcept> parts)
		{
			return parts.Select(builder.HasPart).ToList();
		}

		public static HasPartStatement HasPart(this StatementBuilder builder, IConcept part)
		{
			var statement = new HasPartStatement(null, builder.Subject, part);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<GroupStatement> BelongsToSubjectAreas(this StatementBuilder builder, IEnumerable<IConcept> subjectAreas)
		{
			return subjectAreas.Select(builder.BelongsToSubjectArea).ToList();
		}

		public static GroupStatement BelongsToSubjectArea(this StatementBuilder builder, IConcept subjectArea)
		{
			var statement = new GroupStatement(null, subjectArea, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<GroupStatement> IsSubjectAreaOf(this StatementBuilder builder, IEnumerable<IConcept> concepts)
		{
			return concepts.Select(builder.IsSubjectAreaOf).ToList();
		}

		public static GroupStatement IsSubjectAreaOf(this StatementBuilder builder, IConcept concept)
		{
			var statement = new GroupStatement(null, builder.Subject, concept);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<HasSignStatement> HasSigns(this StatementBuilder builder, IEnumerable<IConcept> signs)
		{
			return signs.Select(builder.HasSign).ToList();
		}

		public static HasSignStatement HasSign(this StatementBuilder builder, IConcept sign)
		{
			var statement = new HasSignStatement(null, builder.Subject, sign);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<HasSignStatement> IsSignOf(this StatementBuilder builder, IEnumerable<IConcept> concepts)
		{
			return concepts.Select(builder.IsSignOf).ToList();
		}

		public static HasSignStatement IsSignOf(this StatementBuilder builder, IConcept concept)
		{
			var statement = new HasSignStatement(null, concept, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<SignValueStatement> HasSignValues(this StatementBuilder builder, IDictionary<IConcept, IConcept> signValues)
		{
			return signValues.Select(signValue => builder.HasSignValue(signValue.Key, signValue.Value)).ToList();
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
