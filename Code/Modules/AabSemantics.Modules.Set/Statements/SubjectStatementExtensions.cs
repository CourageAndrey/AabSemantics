using System.Collections.Generic;
using System.Linq;

using AabSemantics.Statements;

namespace AabSemantics.Modules.Set.Statements
{
	/// <summary>Fluent verbs declaring the set module's statements; each adds the statement to the network immediately.</summary>
	public static class SubjectStatementExtensions
	{
		/// <summary>Declares that the subject is part of another concept.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="wholes">The wholes.</param>
		/// <returns>The created statement or statements, already added to the network.</returns>
		public static List<HasPartStatement> IsPartOf(this StatementBuilder builder, IEnumerable<IConcept> wholes)
		{
			return wholes.Select(builder.IsPartOf).ToList();
		}

		/// <summary>Declares that the subject is part of another concept.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="whole">Containing concept.</param>
		/// <returns>The created statement or statements, already added to the network.</returns>
		public static HasPartStatement IsPartOf(this StatementBuilder builder, IConcept whole)
		{
			var statement = new HasPartStatement(null, whole, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject has several concepts as parts.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="parts">The parts.</param>
		/// <returns>The created statement or statements, already added to the network.</returns>
		public static List<HasPartStatement> HasParts(this StatementBuilder builder, IEnumerable<IConcept> parts)
		{
			return parts.Select(builder.HasPart).ToList();
		}

		/// <summary>Declares that the subject has another concept as a part.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="part">Contained concept.</param>
		/// <returns>The created statement or statements, already added to the network.</returns>
		public static HasPartStatement HasPart(this StatementBuilder builder, IConcept part)
		{
			var statement = new HasPartStatement(null, builder.Subject, part);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject belongs to several subject areas.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="subjectAreas">The subjectAreas.</param>
		/// <returns>The created statement or statements, already added to the network.</returns>
		public static List<GroupStatement> BelongsToSubjectAreas(this StatementBuilder builder, IEnumerable<IConcept> subjectAreas)
		{
			return subjectAreas.Select(builder.BelongsToSubjectArea).ToList();
		}

		/// <summary>Declares that the subject belongs to a subject area.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="subjectArea">The subjectArea.</param>
		/// <returns>The created statement or statements, already added to the network.</returns>
		public static GroupStatement BelongsToSubjectArea(this StatementBuilder builder, IConcept subjectArea)
		{
			var statement = new GroupStatement(null, subjectArea, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject is a subject area containing another concept.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="concepts">The concepts.</param>
		/// <returns>The created statement or statements, already added to the network.</returns>
		public static List<GroupStatement> IsSubjectAreaOf(this StatementBuilder builder, IEnumerable<IConcept> concepts)
		{
			return concepts.Select(builder.IsSubjectAreaOf).ToList();
		}

		/// <summary>Declares that the subject is a subject area containing another concept.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="concept">Concept in question.</param>
		/// <returns>The created statement or statements, already added to the network.</returns>
		public static GroupStatement IsSubjectAreaOf(this StatementBuilder builder, IConcept concept)
		{
			var statement = new GroupStatement(null, builder.Subject, concept);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject has several signs.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="signs">The signs.</param>
		/// <returns>The created statement or statements, already added to the network.</returns>
		public static List<HasSignStatement> HasSigns(this StatementBuilder builder, IEnumerable<IConcept> signs)
		{
			return signs.Select(builder.HasSign).ToList();
		}

		/// <summary>Declares that the subject has a given sign.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="sign">Sign concept.</param>
		/// <returns>The created statement or statements, already added to the network.</returns>
		public static HasSignStatement HasSign(this StatementBuilder builder, IConcept sign)
		{
			var statement = new HasSignStatement(null, builder.Subject, sign);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject is a sign of another concept.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="concepts">The concepts.</param>
		/// <returns>The created statement or statements, already added to the network.</returns>
		public static List<HasSignStatement> IsSignOf(this StatementBuilder builder, IEnumerable<IConcept> concepts)
		{
			return concepts.Select(builder.IsSignOf).ToList();
		}

		/// <summary>Declares that the subject is a sign of another concept.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="concept">Concept in question.</param>
		/// <returns>The created statement or statements, already added to the network.</returns>
		public static HasSignStatement IsSignOf(this StatementBuilder builder, IConcept concept)
		{
			var statement = new HasSignStatement(null, concept, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject has values for several signs.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="signValues">Sign-to-value pairs to declare.</param>
		/// <returns>The created statement or statements, already added to the network.</returns>
		public static List<SignValueStatement> HasSignValues(this StatementBuilder builder, IDictionary<IConcept, IConcept> signValues)
		{
			return signValues.Select(signValue => builder.HasSignValue(signValue.Key, signValue.Value)).ToList();
		}

		/// <summary>Declares that the subject has a value for a given sign.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="sign">Sign concept.</param>
		/// <param name="value">Value concept.</param>
		/// <returns>The created statement or statements, already added to the network.</returns>
		public static SignValueStatement HasSignValue(this StatementBuilder builder, IConcept sign, IConcept value)
		{
			var statement = new SignValueStatement(null, builder.Subject, sign, value);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject is the value of a sign for another concept.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="concept">Concept in question.</param>
		/// <param name="sign">Sign concept.</param>
		/// <returns>The created statement or statements, already added to the network.</returns>
		public static SignValueStatement IsSignValue(this StatementBuilder builder, IConcept concept, IConcept sign)
		{
			var statement = new SignValueStatement(null, concept, sign, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}
	}
}
