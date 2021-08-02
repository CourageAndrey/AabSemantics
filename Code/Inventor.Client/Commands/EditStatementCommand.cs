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

		public EditStatementCommand(ILanguage language, StatementViewModel viewModel, ISemanticNetwork semanticNetwork)
			: base(semanticNetwork)
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
