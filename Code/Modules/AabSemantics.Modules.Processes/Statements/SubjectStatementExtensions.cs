using System.Collections.Generic;
using System.Linq;

using AabSemantics.Modules.Processes.Concepts;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Processes.Statements
{
	/// <summary>Fluent verbs declaring process sequence statements; each adds the statement to the network immediately.</summary>
	public static class SubjectStatementExtensions
	{
		/// <summary>Declares that the subject starts after each of several processes started.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Processes to relate the subject to.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ProcessesStatement> StartsAfterOthersStarted(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.StartsAfterOtherStarted).ToList();
		}

		/// <summary>Declares that the subject starts after the other process started.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Process to relate the subject to.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ProcessesStatement StartsAfterOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.StartsAfterOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject starts when each of several processes starts.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Processes to relate the subject to.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ProcessesStatement> StartsWhenOthersStarted(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.StartsWhenOtherStarted).ToList();
		}

		/// <summary>Declares that the subject starts when the other process starts.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Process to relate the subject to.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ProcessesStatement StartsWhenOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.StartsWhenOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject starts before each of several processes starts.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Processes to relate the subject to.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ProcessesStatement> StartsBeforeOthersStarted(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.StartsBeforeOtherStarted).ToList();
		}

		/// <summary>Declares that the subject starts before the other process starts.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Process to relate the subject to.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ProcessesStatement StartsBeforeOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.StartsBeforeOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject finishes after each of several processes started.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Processes to relate the subject to.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ProcessesStatement> FinishesAfterOthersStarted(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.FinishesAfterOtherStarted).ToList();
		}

		/// <summary>Declares that the subject finishes after the other process started.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Process to relate the subject to.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ProcessesStatement FinishesAfterOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.FinishesAfterOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject finishes when each of several processes starts.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Processes to relate the subject to.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ProcessesStatement> FinishesWhenOthersStarted(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.FinishesWhenOtherStarted).ToList();
		}

		/// <summary>Declares that the subject finishes when the other process starts.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Process to relate the subject to.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ProcessesStatement FinishesWhenOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.FinishesWhenOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject finishes before each of several processes starts.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Processes to relate the subject to.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ProcessesStatement> FinishesBeforeOthersStarted(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.FinishesBeforeOtherStarted).ToList();
		}

		/// <summary>Declares that the subject finishes before the other process starts.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Process to relate the subject to.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ProcessesStatement FinishesBeforeOtherStarted(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.FinishesBeforeOtherStarted);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject starts after each of several processes finished.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Processes to relate the subject to.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ProcessesStatement> StartsAfterOthersFinished(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.StartsAfterOtherFinished).ToList();
		}

		/// <summary>Declares that the subject starts after the other process finished.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Process to relate the subject to.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ProcessesStatement StartsAfterOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.StartsAfterOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject starts when each of several processes finishes.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Processes to relate the subject to.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ProcessesStatement> StartsWhenOthersFinished(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.StartsWhenOtherFinished).ToList();
		}

		/// <summary>Declares that the subject starts when the other process finishes.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Process to relate the subject to.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ProcessesStatement StartsWhenOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.StartsWhenOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject starts before each of several processes finishes.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Processes to relate the subject to.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ProcessesStatement> StartsBeforeOthersFinished(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.StartsBeforeOtherFinished).ToList();
		}

		/// <summary>Declares that the subject starts before the other process finishes.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Process to relate the subject to.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ProcessesStatement StartsBeforeOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.StartsBeforeOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject finishes after each of several processes finished.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Processes to relate the subject to.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ProcessesStatement> FinishesAfterOthersFinished(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.FinishesAfterOtherFinished).ToList();
		}

		/// <summary>Declares that the subject finishes after the other process finished.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Process to relate the subject to.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ProcessesStatement FinishesAfterOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.FinishesAfterOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject finishes when each of several processes finishes.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Processes to relate the subject to.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ProcessesStatement> FinishesWhenOthersFinished(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.FinishesWhenOtherFinished).ToList();
		}

		/// <summary>Declares that the subject finishes when the other process finishes.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Process to relate the subject to.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ProcessesStatement FinishesWhenOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.FinishesWhenOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject finishes before each of several processes finishes.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Processes to relate the subject to.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ProcessesStatement> FinishesBeforeOthersFinished(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.FinishesBeforeOtherFinished).ToList();
		}

		/// <summary>Declares that the subject finishes before the other process finishes.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Process to relate the subject to.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ProcessesStatement FinishesBeforeOtherFinished(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.FinishesBeforeOtherFinished);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject causes each of several processes.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Processes to relate the subject to.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ProcessesStatement> Causes(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.Causes).ToList();
		}

		/// <summary>Declares that the subject causes the other process.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Process to relate the subject to.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ProcessesStatement Causes(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.Causes);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject is caused by each of several processes.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Processes to relate the subject to.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ProcessesStatement> IsCausedBy(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.IsCausedBy).ToList();
		}

		/// <summary>Declares that the subject is caused by the other process.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Process to relate the subject to.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ProcessesStatement IsCausedBy(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.IsCausedBy);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares that the subject runs simultaneously with each of several processes.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Processes to relate the subject to.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ProcessesStatement> SimultaneousWith(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.SimultaneousWith).ToList();
		}

		/// <summary>Declares that the subject runs simultaneously with the other process.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Process to relate the subject to.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ProcessesStatement SimultaneousWith(this StatementBuilder builder, IConcept other)
		{
			var statement = new ProcessesStatement(null, builder.Subject, other, SequenceSigns.SimultaneousWith);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}
	}
}
