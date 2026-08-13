using System.Linq;

using AabSemantics.Extensions.WPF.TreeNodes;

namespace AabSemantics.Extensions.WPF.Commands
{
	/// <summary>Undoable command that adds a statement.</summary>
	public class AddStatementCommand : BaseEditCommand
	{
		#region Properties

		/// <summary>View model holding the edited values.</summary>
		public StatementViewModel ViewModel
		{ get; }

		/// <summary>The item created by this command.</summary>
		public IStatement NewItem
		{ get; private set; }

		#endregion

		/// <summary>Creates the command.</summary>
		/// <param name="viewModel">View model holding the edited values.</param>
		/// <param name="semanticNetworkNode">Tree node representing the network.</param>
		/// <param name="application">The hosting application.</param>
		public AddStatementCommand(StatementViewModel viewModel, SemanticNetworkNode semanticNetworkNode, IInventorApplication application)
			: base(semanticNetworkNode, application)
		{
			ViewModel = viewModel;
		}

		/// <summary>Performs the edit.</summary>
		public override void Apply()
		{
			if (NewItem == null)
			{
				NewItem = (IStatement) ViewModel.ApplyCreate(SemanticNetworkNode.SemanticNetwork);
			}
			else
			{
				SemanticNetwork.Statements.Add(NewItem);
			}
			SemanticNetworkNode.Statements.Children.Add(new StatementNode(NewItem, Application));
		}

		/// <summary>Reverses the edit, restoring the previous state.</summary>
		public override void Rollback()
		{
			SemanticNetwork.Statements.Remove(NewItem);
			var statements = SemanticNetworkNode.Statements.Children;
			statements.Remove(statements.OfType<StatementNode>().First(r => r.Statement == NewItem));
		}
	}
}
