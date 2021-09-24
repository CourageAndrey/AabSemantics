using Inventor.Client.TreeNodes;
using Inventor.Core;

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

		public EditStatementCommand(ILanguage language, StatementViewModel viewModel, SemanticNetworkNode semanticNetworkNode)
			: base(semanticNetworkNode)
		{
			ViewModel = viewModel;
			PreviousVersion = (StatementViewModel) ViewModels.Factory.CreateStatementByInstance(viewModel.BoundStatement, language);
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
