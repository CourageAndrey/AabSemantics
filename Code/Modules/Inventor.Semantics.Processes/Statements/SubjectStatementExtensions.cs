using Inventor.Semantics.Statements;
using Inventor.Semantics.Processes.Concepts;

namespace Inventor.Semantics.Processes.Statements
{
	public static class SubjectStatementExtensions
	{
		public static ProcessesStatement StartsAfterOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.StartsAfterOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement StartsWhenOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.StartsWhenOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement StartsBeforeOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.StartsBeforeOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement FinishesAfterOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.FinishesAfterOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement FinishesWhenOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.FinishesWhenOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement FinishesBeforeOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.FinishesBeforeOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement StartsAfterOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.StartsAfterOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement StartsWhenOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.StartsWhenOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement StartsBeforeOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.StartsBeforeOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement FinishesAfterOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.FinishesAfterOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement FinishesWhenOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.FinishesWhenOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement FinishesBeforeOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.FinishesBeforeOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement Causes(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.Causes);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement IsCausedBy(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.IsCausedBy);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static ProcessesStatement SimultaneousWith(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.SimultaneousWith);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}
	}
}
