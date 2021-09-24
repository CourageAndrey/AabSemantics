using Inventor.Client.TreeNodes;
using Inventor.Core;

namespace Inventor.Client.Commands
{
	public class AddStatementCommand : BaseEditCommand
	{
		#region Properties

		public StatementViewModel ViewModel
		{ get; }

		public IStatement NewItem
		{ get; private set; }

		#endregion

		public AddStatementCommand(StatementViewModel viewModel, SemanticNetworkNode semanticNetworkNode, InventorApplication application)
			: base(semanticNetworkNode, application)
		{
			ViewModel = viewModel;
		}

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
		}

		public override void Rollback()
		{
			SemanticNetwork.Statements.Remove(NewItem);
		}
	}
}
