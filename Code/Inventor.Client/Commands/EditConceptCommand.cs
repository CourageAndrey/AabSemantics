using Inventor.Client.TreeNodes;

namespace Inventor.Client.Commands
{
	public class EditConceptCommand : BaseEditCommand
	{
		#region Properties

		public ViewModels.Concept ViewModel
		{ get; }

		public ViewModels.Concept PreviousVersion
		{ get; }

		#endregion

		public EditConceptCommand(ViewModels.Concept viewModel, SemanticNetworkNode semanticNetworkNode)
			: base(semanticNetworkNode)
		{
			ViewModel = viewModel;
			PreviousVersion = new ViewModels.Concept(ViewModel.BoundObject);
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
