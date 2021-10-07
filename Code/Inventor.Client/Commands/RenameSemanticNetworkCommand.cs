using Inventor.Client.TreeNodes;
using Inventor.Client.ViewModels;

namespace Inventor.Client.Commands
{
	public class RenameSemanticNetworkCommand : BaseEditCommand
	{
		#region Properties

		public LocalizedString PreviousName
		{ get; }

		public LocalizedString NewName
		{ get; }

		#endregion

		public RenameSemanticNetworkCommand(SemanticNetworkNode semanticNetworkNode, LocalizedString newName, IInventorApplication application)
			: base(semanticNetworkNode, application)
		{
			PreviousName = LocalizedString.From(SemanticNetwork.Name);
			NewName = newName;
		}

		public override void Apply()
		{
			NewName.Apply(SemanticNetwork.Name);
		}

		public override void Rollback()
		{
			PreviousName.Apply(SemanticNetwork.Name);
		}
	}
}
