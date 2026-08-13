using System.Collections.Generic;
using System.Linq;

using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Mathematics.Statements
{
	/// <summary>Fluent verbs declaring comparison statements; each adds the statement to the network immediately.</summary>
	public static class SubjectStatementExtensions
	{
		/// <summary>Declares the subject to be equal to several concepts.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Concepts to compare the subject against.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ComparisonStatement> IsEqualTo(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.IsEqualTo).ToList();
		}

		/// <summary>Declares the subject to be equal to another concept.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Concept to compare the subject against.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ComparisonStatement IsEqualTo(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(null, builder.Subject, other, ComparisonSigns.IsEqualTo);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares the subject to be not equal to several concepts.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Concepts to compare the subject against.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ComparisonStatement> IsNotEqualTo(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.IsNotEqualTo).ToList();
		}

		/// <summary>Declares the subject to be not equal to another concept.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Concept to compare the subject against.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ComparisonStatement IsNotEqualTo(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(null, builder.Subject, other, ComparisonSigns.IsNotEqualTo);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares the subject to be greater than several concepts.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Concepts to compare the subject against.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ComparisonStatement> IsGreaterThan(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.IsGreaterThan).ToList();
		}

		/// <summary>Declares the subject to be greater than another concept.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Concept to compare the subject against.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ComparisonStatement IsGreaterThan(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(null, builder.Subject, other, ComparisonSigns.IsGreaterThan);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares the subject to be greater than or equal to several concepts.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Concepts to compare the subject against.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ComparisonStatement> IsGreaterThanOrEqualTo(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.IsGreaterThanOrEqualTo).ToList();
		}

		/// <summary>Declares the subject to be greater than or equal to another concept.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Concept to compare the subject against.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ComparisonStatement IsGreaterThanOrEqualTo(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(null, builder.Subject, other, ComparisonSigns.IsGreaterThanOrEqualTo);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares the subject to be less than several concepts.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Concepts to compare the subject against.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ComparisonStatement> IsLessThan(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.IsLessThan).ToList();
		}

		/// <summary>Declares the subject to be less than another concept.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Concept to compare the subject against.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ComparisonStatement IsLessThan(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(null, builder.Subject, other, ComparisonSigns.IsLessThan);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Declares the subject to be less than or equal to several concepts.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="others">Concepts to compare the subject against.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ComparisonStatement> IsLessThanOrEqualTo(this StatementBuilder builder, IEnumerable<IConcept> others)
		{
			return others.Select(builder.IsLessThanOrEqualTo).ToList();
		}

		/// <summary>Declares the subject to be less than or equal to another concept.</summary>
		/// <param name="builder">Builder carrying the network and the subject.</param>
		/// <param name="other">Concept to compare the subject against.</param>
		/// <returns>The created statement, already added to the network.</returns>
		public static ComparisonStatement IsLessThanOrEqualTo(this StatementBuilder builder, IConcept other)
		{
			var statement = new ComparisonStatement(null, builder.Subject, other, ComparisonSigns.IsLessThanOrEqualTo);
			builder.SemanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>
		/// Declares a sequence of values ordered by one sign, relating <em>every</em> pair rather
		/// than only the neighbours, so the ordering needs no transitive inference later.
		/// </summary>
		/// <param name="semanticNetwork">Network to add the statements to.</param>
		/// <param name="numbers">Values in sequence order.</param>
		/// <param name="comparisonSign">Sign relating each earlier value to each later one.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ComparisonStatement> DefineSequence(this ISemanticNetwork semanticNetwork, IEnumerable<IConcept> numbers, IConcept comparisonSign)
		{
			var comparisons = new List<ComparisonStatement>();
			IConcept leftValue = null;

			foreach (var number in numbers)
			{
				if (leftValue == null)
				{
					leftValue = number;
				}
				else
				{
					var rightValue = number;

					var comparison = new ComparisonStatement(null, leftValue, rightValue, comparisonSign);
					semanticNetwork.Statements.Add(comparison);
					comparisons.Add(comparison);

					leftValue = rightValue;
				}
			}

			return comparisons;
		}

		/// <summary>Declares a strictly increasing sequence of values, comparing each neighbouring pair.</summary>
		/// <param name="semanticNetwork">Network to add the statements to.</param>
		/// <param name="numbers">Values in sequence order.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ComparisonStatement> DefineAscendingSequence(this ISemanticNetwork semanticNetwork, IEnumerable<IConcept> numbers)
		{
			return DefineSequence(semanticNetwork, numbers, ComparisonSigns.IsLessThan);
		}

		/// <summary>Declares a strictly decreasing sequence of values, comparing each neighbouring pair.</summary>
		/// <param name="semanticNetwork">Network to add the statements to.</param>
		/// <param name="numbers">Values in sequence order.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ComparisonStatement> DefineDescendingSequence(this ISemanticNetwork semanticNetwork, IEnumerable<IConcept> numbers)
		{
			return DefineSequence(semanticNetwork, numbers, ComparisonSigns.IsGreaterThan);
		}

		/// <summary>Declares a non-increasing sequence of values, comparing each neighbouring pair.</summary>
		/// <param name="semanticNetwork">Network to add the statements to.</param>
		/// <param name="numbers">Values in sequence order.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ComparisonStatement> DefineNotAscendingSequence(this ISemanticNetwork semanticNetwork, IEnumerable<IConcept> numbers)
		{
			return DefineSequence(semanticNetwork, numbers, ComparisonSigns.IsGreaterThanOrEqualTo);
		}

		/// <summary>Declares a non-decreasing sequence of values, comparing each neighbouring pair.</summary>
		/// <param name="semanticNetwork">Network to add the statements to.</param>
		/// <param name="numbers">Values in sequence order.</param>
		/// <returns>The created statements, already added to the network.</returns>
		public static List<ComparisonStatement> DefineNotDescendingSequence(this ISemanticNetwork semanticNetwork, IEnumerable<IConcept> numbers)
		{
			return DefineSequence(semanticNetwork, numbers, ComparisonSigns.IsLessThanOrEqualTo);
		}
	}
}
