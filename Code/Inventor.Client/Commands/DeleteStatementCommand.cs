using Inventor.Client.TreeNodes;
using Inventor.Core;

namespace Inventor.Client.Commands
{
	public class DeleteStatementCommand : BaseEditCommand
	{
		#region Properties

		public IStatement Statement
		{ get; }

		#endregion

		public DeleteStatementCommand(IStatement statement, SemanticNetworkNode semanticNetworkNode)
			: base(semanticNetworkNode)
		{
			Statement = statement;
		}

		public override void Apply()
		{
			SemanticNetwork.Statements.Remove(Statement);
		}

		public override void Rollback()
		{
			SemanticNetwork.Statements.Add(Statement);
		}
	}
}
