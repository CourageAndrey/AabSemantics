using System;

namespace AabSemantics.Serialization
{
	public class StatementIdResolver
	{
		public ISemanticNetwork SemanticNetwork
		{ get; }

		public StatementIdResolver(ISemanticNetwork semanticNetwork)
		{
			SemanticNetwork = semanticNetwork;
		}

		public Boolean TryGetStatement(String id, out IStatement statement)
		{
			return SemanticNetwork.Statements.TryGetValue(id, out statement);
		}
	}
}
