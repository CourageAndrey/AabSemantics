using Inventor.Semantics.WPF.TreeNodes;

namespace Inventor.Semantics.WPF.Commands
{
	public class EditConceptCommand : BaseEditCommand
	{
		#region Properties

		public ViewModels.Concept ViewModel
		{ get; }

		public ViewModels.Concept PreviousVersion
		{ get; }

		#endregion

		public EditConceptCommand(ViewModels.Concept viewModel, ViewModels.Concept previousVersion, SemanticNetworkNode semanticNetworkNode, IInventorApplication application)
			: base(semanticNetworkNode, application)
		{
			ViewModel = viewModel;
			PreviousVersion = previousVersion;
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
