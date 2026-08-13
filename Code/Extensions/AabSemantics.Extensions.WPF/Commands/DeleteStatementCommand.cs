using System.Linq;

using AabSemantics.Extensions.WPF.TreeNodes;
using AabSemantics.Utils;

namespace AabSemantics.Extensions.WPF.Commands
{
	/// <summary>Undoable command that deletes a statement.</summary>
	public class DeleteStatementCommand : BaseEditCommand
	{
		#region Properties

		/// <summary>The statement being edited.</summary>
		public IStatement Statement
		{ get; }

		/// <summary>Position the item occupied before deletion, so undo can restore the order.</summary>
		public int Index
		{ get; private set; }

		#endregion

		/// <summary>Creates the command.</summary>
		/// <param name="statement">The statement being edited.</param>
		/// <param name="semanticNetworkNode">Tree node representing the network.</param>
		/// <param name="application">The hosting application.</param>
		public DeleteStatementCommand(IStatement statement, SemanticNetworkNode semanticNetworkNode, IInventorApplication application)
			: base(semanticNetworkNode, application)
		{
			Statement = statement;
		}

		/// <summary>Performs the edit.</summary>
		public override void Apply()
		{
			Index = SemanticNetwork.Statements.IndexOf(Statement);
			SemanticNetwork.Statements.Remove(Statement);
			var statements = SemanticNetworkNode.Statements.Children;
			statements.Remove(statements.OfType<StatementNode>().First(r => r.Statement == Statement));
		}

		/// <summary>Reverses the edit, restoring the previous state.</summary>
		public override void Rollback()
		{
			SemanticNetwork.Statements.Add(Statement);
			SemanticNetworkNode.Statements.Children.Insert(Index, new StatementNode(Statement, Application));
		}
	}
}
