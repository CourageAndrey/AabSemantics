using System;

using Inventor.Core.Statements;

namespace Inventor.Core
{
	public class StatementBuilder
	{
		public ISemanticNetwork SemanticNetwork
		{ get; }

		public IConcept Subject
		{ get; }

		public StatementBuilder(ISemanticNetwork semanticNetwork, IConcept subject)
		{
			if (semanticNetwork == null) throw new ArgumentNullException(nameof(semanticNetwork));
			if (subject == null) throw new ArgumentNullException(nameof(subject));

			SemanticNetwork = semanticNetwork;
			Subject = subject;
		}
	}

	public static class SubjectStatementExtensions
	{
		public static StatementBuilder DeclareThat(this ISemanticNetwork semanticNetwork, IConcept subject)
		{
			return new StatementBuilder(semanticNetwork, subject);
		}

		public static HasPartStatement IsPartOf(this StatementBuilder builder, IConcept whole)
		{
			var statement = new HasPartStatement(whole, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static HasPartStatement HasPart(this StatementBuilder builder, IConcept part)
		{
			var statement = new HasPartStatement(builder.Subject, part);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static GroupStatement BelongsToSubjectArea(this StatementBuilder builder, IConcept subjectArea)
		{
			var statement = new GroupStatement(subjectArea, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static GroupStatement IsSubjectAreaOf(this StatementBuilder builder, IConcept concept)
		{
			var statement = new GroupStatement(builder.Subject, concept);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static IsStatement IsAncestorOf(this StatementBuilder builder, IConcept descendant)
		{
			var statement = new IsStatement(builder.Subject, descendant);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static IsStatement IsDescendantOf(this StatementBuilder builder, IConcept ancestor)
		{
			var statement = new IsStatement(ancestor, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static HasSignStatement HasSign(this StatementBuilder builder, IConcept sign)
		{
			var statement = new HasSignStatement(builder.Subject, sign);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static HasSignStatement IsSignOf(this StatementBuilder builder, IConcept concept)
		{
			var statement = new HasSignStatement(concept, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static SignValueStatement HasSignValue(this StatementBuilder builder, IConcept sign, IConcept value)
		{
			var statement = new SignValueStatement(builder.Subject, sign, value);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static SignValueStatement IsSignValue(this StatementBuilder builder, IConcept concept, IConcept sign)
		{
			var statement = new SignValueStatement(concept, sign, builder.Subject);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ComparisonStatement IsEqualTo(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(builder.Subject, other, ComparisonSigns.IsEqualTo);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ComparisonStatement IsNotEqualTo(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(builder.Subject, other, ComparisonSigns.IsNotEqualTo);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ComparisonStatement IsGreaterThan(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(builder.Subject, other, ComparisonSigns.IsGreaterThan);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ComparisonStatement IsGreaterThanOrEqualTo(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(builder.Subject, other, ComparisonSigns.IsGreaterThanOrEqualTo);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ComparisonStatement IsLessThan(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(builder.Subject, other, ComparisonSigns.IsLessThan);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ComparisonStatement IsLessThanOrEqualTo(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(builder.Subject, other, ComparisonSigns.IsLessThanOrEqualTo);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement StartsAfterOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(builder.Subject, other, SequenceSigns.StartsAfterOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement StartsWhenOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(builder.Subject, other, SequenceSigns.StartsWhenOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement StartsBeforeOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(builder.Subject, other, SequenceSigns.StartsBeforeOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement FinishesAfterOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(builder.Subject, other, SequenceSigns.FinishesAfterOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement FinishesWhenOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(builder.Subject, other, SequenceSigns.FinishesWhenOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement FinishesBeforeOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(builder.Subject, other, SequenceSigns.FinishesBeforeOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement StartsAfterOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(builder.Subject, other, SequenceSigns.StartsAfterOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement StartsWhenOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(builder.Subject, other, SequenceSigns.StartsWhenOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement StartsBeforeOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(builder.Subject, other, SequenceSigns.StartsBeforeOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement FinishesAfterOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(builder.Subject, other, SequenceSigns.FinishesAfterOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement FinishesWhenOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(builder.Subject, other, SequenceSigns.FinishesWhenOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement FinishesBeforeOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(builder.Subject, other, SequenceSigns.FinishesBeforeOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement Causes(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(builder.Subject, other, SequenceSigns.Causes);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement IsCausedBy(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(builder.Subject, other, SequenceSigns.IsCausedBy);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement SimultaneousWith(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(builder.Subject, other, SequenceSigns.SimultaneousWith);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}
	}
}
