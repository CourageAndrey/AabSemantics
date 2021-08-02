using Inventor.Core;

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

		public EditConceptCommand(ViewModels.Concept viewModel, ISemanticNetwork semanticNetwork)
			: base(semanticNetwork)
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
