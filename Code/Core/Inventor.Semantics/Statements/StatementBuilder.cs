using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Statements
{
	public class StatementBuilder
	{
		public ISemanticNetwork SemanticNetwork
		{ get; }

		public IConcept Subject
		{ get; }

		public StatementBuilder(ISemanticNetwork semanticNetwork, IConcept subject)
		{
			SemanticNetwork = semanticNetwork.EnsureNotNull(nameof(semanticNetwork));
			Subject = subject.EnsureNotNull(nameof(subject));
		}
	}

	public static class SubjectStatementExtensions
	{
		public static StatementBuilder DeclareThat(this ISemanticNetwork semanticNetwork, IConcept subject)
		{
			return new StatementBuilder(semanticNetwork, subject);
		}
	}
}
