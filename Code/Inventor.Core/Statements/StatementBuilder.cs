using System;

namespace Inventor.Core.Statements
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
	}
}
