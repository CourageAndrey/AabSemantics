using AabSemantics.Extensions.WPF.TreeNodes;

namespace AabSemantics.Extensions.WPF.Commands
{
	/// <summary>Undoable command that applies edits to a statement.</summary>
	public class EditStatementCommand : BaseEditCommand
	{
		#region Properties

		/// <summary>View model holding the edited values.</summary>
		public StatementViewModel ViewModel
		{ get; }

		/// <summary>Snapshot of the item before the edit, used to roll back.</summary>
		public StatementViewModel PreviousVersion
		{ get; set; }

		#endregion

		/// <summary>Creates the command.</summary>
		/// <param name="viewModel">View model holding the edited values.</param>
		/// <param name="previousVersion">The previousVersion.</param>
		/// <param name="semanticNetworkNode">Tree node representing the network.</param>
		/// <param name="application">The hosting application.</param>
		public EditStatementCommand(StatementViewModel viewModel, StatementViewModel previousVersion, SemanticNetworkNode semanticNetworkNode, IInventorApplication application)
			: base(semanticNetworkNode, application)
		{
			ViewModel = viewModel;
			PreviousVersion = previousVersion;
		}

		/// <summary>Performs the edit.</summary>
		public override void Apply()
		{
			ViewModel.ApplyUpdate();
		}

		/// <summary>Reverses the edit, restoring the previous state.</summary>
		public override void Rollback()
		{
			PreviousVersion.ApplyUpdate();
		}
	}
}
