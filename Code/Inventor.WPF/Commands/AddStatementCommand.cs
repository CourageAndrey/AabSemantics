using System.Linq;

using Inventor.WPF.TreeNodes;
using Inventor.Core;

namespace Inventor.WPF.Commands
{
	public class AddStatementCommand : BaseEditCommand
	{
		#region Properties

		public StatementViewModel ViewModel
		{ get; }

		public IStatement NewItem
		{ get; private set; }

		#endregion

		public AddStatementCommand(StatementViewModel viewModel, SemanticNetworkNode semanticNetworkNode, IInventorApplication application)
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
			SemanticNetworkNode.Statements.Children.Add(new StatementNode(NewItem, Application));
		}

		public override void Rollback()
		{
			SemanticNetwork.Statements.Remove(NewItem);
			var statements = SemanticNetworkNode.Statements.Children;
			statements.Remove(statements.OfType<StatementNode>().First(r => r.Statement == NewItem));
		}
	}
}
