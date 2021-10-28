using System.Linq;

using Inventor.WPF.TreeNodes;
using Inventor.Semantics;
using Inventor.Semantics.Utils;

namespace Inventor.WPF.Commands
{
	public class DeleteStatementCommand : BaseEditCommand
	{
		#region Properties

		public IStatement Statement
		{ get; }

		public int Index
		{ get; private set; }

		#endregion

		public DeleteStatementCommand(IStatement statement, SemanticNetworkNode semanticNetworkNode, IInventorApplication application)
			: base(semanticNetworkNode, application)
		{
			Statement = statement;
		}

		public override void Apply()
		{
			Index = SemanticNetwork.Statements.IndexOf(Statement);
			SemanticNetwork.Statements.Remove(Statement);
			var statements = SemanticNetworkNode.Statements.Children;
			statements.Remove(statements.OfType<StatementNode>().First(r => r.Statement == Statement));
		}

		public override void Rollback()
		{
			SemanticNetwork.Statements.Add(Statement);
			SemanticNetworkNode.Statements.Children.Insert(Index, new StatementNode(Statement, Application));
		}
	}
}
