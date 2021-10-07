using Inventor.Client.TreeNodes;

namespace Inventor.Client.Commands
{
	public class EditStatementCommand : BaseEditCommand
	{
		#region Properties

		public StatementViewModel ViewModel
		{ get; }

		public StatementViewModel PreviousVersion
		{ get; set; }

		#endregion

		public EditStatementCommand(StatementViewModel viewModel, SemanticNetworkNode semanticNetworkNode, InventorApplication application)
			: base(semanticNetworkNode, application)
		{
			ViewModel = viewModel;
			PreviousVersion = ViewModels.ViewModelFactory.CreateStatementByInstance(viewModel.BoundStatement, application.CurrentLanguage);
		}

		public override void Apply()
		{
			ViewModel.ApplyUpdate();
		}

		public override void Rollback()
		{
			PreviousVersion.ApplyUpdate();
		}
	}
}
