using System;

namespace AabSemantics.Serialization
{
	/// <summary>
	/// Turns statement identifiers back into statements while deserializing, so that a
	/// deserialized question can reference a statement the network already holds instead of
	/// creating a duplicate.
	/// </summary>
	public class StatementIdResolver
	{
		/// <summary>Network the statements are looked up in.</summary>
		public ISemanticNetwork SemanticNetwork
		{ get; }

		/// <summary>Creates a resolver over a network's statements.</summary>
		/// <param name="semanticNetwork">Network to look statements up in.</param>
		public StatementIdResolver(ISemanticNetwork semanticNetwork)
		{
			SemanticNetwork = semanticNetwork;
		}

		/// <summary>Looks a statement up by identifier.</summary>
		/// <param name="id">Statement identifier.</param>
		/// <param name="statement">Receives the matching statement, or <c>null</c>.</param>
		/// <returns><c>true</c> if the network holds such a statement.</returns>
		public Boolean TryGetStatement(String id, out IStatement statement)
		{
			return SemanticNetwork.Statements.TryGetValue(id, out statement);
		}
	}
}
