using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Statements;
using Inventor.Semantics.Processes.Concepts;

namespace Inventor.Semantics.Processes.Statements
{
	public static class SubjectStatementExtensions
	{
		public static List<ProcessesStatement> StartsAfterOthersStarted(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.StartsAfterOtherStarted).ToList();
		}

		public static ProcessesStatement StartsAfterOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.StartsAfterOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ProcessesStatement> StartsWhenOthersStarted(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.StartsWhenOtherStarted).ToList();
		}

		public static ProcessesStatement StartsWhenOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.StartsWhenOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ProcessesStatement> StartsBeforeOthersStarted(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.StartsBeforeOtherStarted).ToList();
		}

		public static ProcessesStatement StartsBeforeOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.StartsBeforeOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ProcessesStatement> FinishesAfterOthersStarted(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.FinishesAfterOtherStarted).ToList();
		}

		public static ProcessesStatement FinishesAfterOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.FinishesAfterOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ProcessesStatement> FinishesWhenOthersStarted(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.FinishesWhenOtherStarted).ToList();
		}

		public static ProcessesStatement FinishesWhenOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.FinishesWhenOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ProcessesStatement> FinishesBeforeOthersStarted(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.FinishesBeforeOtherStarted).ToList();
		}

		public static ProcessesStatement FinishesBeforeOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.FinishesBeforeOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ProcessesStatement> StartsAfterOthersFinished(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.StartsAfterOtherFinished).ToList();
		}

		public static ProcessesStatement StartsAfterOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.StartsAfterOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ProcessesStatement> StartsWhenOthersFinished(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.StartsWhenOtherFinished).ToList();
		}

		public static ProcessesStatement StartsWhenOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.StartsWhenOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ProcessesStatement> StartsBeforeOthersFinished(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.StartsBeforeOtherFinished).ToList();
		}

		public static ProcessesStatement StartsBeforeOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.StartsBeforeOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ProcessesStatement> FinishesAfterOthersFinished(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.FinishesAfterOtherFinished).ToList();
		}

		public static ProcessesStatement FinishesAfterOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.FinishesAfterOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ProcessesStatement> FinishesWhenOthersFinished(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.FinishesWhenOtherFinished).ToList();
		}

		public static ProcessesStatement FinishesWhenOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.FinishesWhenOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ProcessesStatement> FinishesBeforeOthersFinished(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.FinishesBeforeOtherFinished).ToList();
		}

		public static ProcessesStatement FinishesBeforeOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.FinishesBeforeOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ProcessesStatement> Causes(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.Causes).ToList();
		}

		public static ProcessesStatement Causes(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.Causes);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ProcessesStatement> IsCausedBy(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.IsCausedBy).ToList();
		}

		public static ProcessesStatement IsCausedBy(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.IsCausedBy);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		public static List<ProcessesStatement> SimultaneousWith(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.SimultaneousWith).ToList();
		}

		public static ProcessesStatement SimultaneousWith(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.SimultaneousWith);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}
	}
}
