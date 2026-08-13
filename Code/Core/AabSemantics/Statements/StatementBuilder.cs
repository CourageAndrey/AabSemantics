using AabSemantics.Utils;

namespace AabSemantics.Statements
{
	/// <summary>
	/// Carries a network and a subject between the two halves of the fluent statement syntax.
	/// Modules extend it with the verbs their statement types support.
	/// </summary>
	public class StatementBuilder
	{
		/// <summary>Network the statement will be added to.</summary>
		public ISemanticNetwork SemanticNetwork
		{ get; }

		/// <summary>Concept the statement will be about.</summary>
		public IConcept Subject
		{ get; }

		/// <summary>Creates a builder.</summary>
		/// <param name="semanticNetwork">Network the statement will be added to.</param>
		/// <param name="subject">Concept the statement will be about.</param>
		/// <exception cref="System.ArgumentNullException">Any argument is <c>null</c>.</exception>
		public StatementBuilder(ISemanticNetwork semanticNetwork, IConcept subject)
		{
			SemanticNetwork = semanticNetwork.EnsureNotNull(nameof(semanticNetwork));
			Subject = subject.EnsureNotNull(nameof(subject));
		}
	}

	/// <summary>Entry point of the fluent statement syntax.</summary>
	public static class SubjectStatementExtensions
	{
		/// <summary>Begins a fluent statement about a concept.</summary>
		/// <param name="semanticNetwork">Network the statement will be added to.</param>
		/// <param name="subject">Concept the statement will be about.</param>
		/// <returns>A builder the verb is then called on.</returns>
		public static StatementBuilder DeclareThat(this ISemanticNetwork semanticNetwork, IConcept subject)
		{
			return new StatementBuilder(semanticNetwork, subject);
		}
	}
}
