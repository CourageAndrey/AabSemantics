using AabSemantics.Extensions.WPF.TreeNodes;

namespace AabSemantics.Extensions.WPF.Commands
{
	/// <summary>Undoable command that applies edits to a concept.</summary>
	public class EditConceptCommand : BaseEditCommand
	{
		#region Properties

		/// <summary>View model holding the edited values.</summary>
		public ViewModels.Concept ViewModel
		{ get; }

		/// <summary>Snapshot of the item before the edit, used to roll back.</summary>
		public ViewModels.Concept PreviousVersion
		{ get; }

		#endregion

		/// <summary>Creates the command.</summary>
		/// <param name="viewModel">View model holding the edited values.</param>
		/// <param name="previousVersion">The previousVersion.</param>
		/// <param name="semanticNetworkNode">Tree node representing the network.</param>
		/// <param name="application">The hosting application.</param>
		public EditConceptCommand(ViewModels.Concept viewModel, ViewModels.Concept previousVersion, SemanticNetworkNode semanticNetworkNode, IInventorApplication application)
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
