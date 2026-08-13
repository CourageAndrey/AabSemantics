using AabSemantics.Extensions.WPF.TreeNodes;
using AabSemantics.Extensions.WPF.ViewModels;

namespace AabSemantics.Extensions.WPF.Commands
{
	/// <summary>Undoable command that renames the knowledge base.</summary>
	public class RenameSemanticNetworkCommand : BaseEditCommand
	{
		#region Properties

		/// <summary>The name before the rename, used to roll back.</summary>
		public LocalizedString PreviousName
		{ get; }

		/// <summary>The name to apply.</summary>
		public LocalizedString NewName
		{ get; }

		#endregion

		/// <summary>Creates the command.</summary>
		/// <param name="semanticNetworkNode">Tree node representing the network.</param>
		/// <param name="newName">The newName.</param>
		/// <param name="application">The hosting application.</param>
		public RenameSemanticNetworkCommand(SemanticNetworkNode semanticNetworkNode, LocalizedString newName, IInventorApplication application)
			: base(semanticNetworkNode, application)
		{
			PreviousName = LocalizedString.From(SemanticNetwork.Name);
			NewName = newName;
		}

		/// <summary>Performs the edit.</summary>
		public override void Apply()
		{
			NewName.Apply(SemanticNetwork.Name);
		}

		/// <summary>Reverses the edit, restoring the previous state.</summary>
		public override void Rollback()
		{
			PreviousName.Apply(SemanticNetwork.Name);
		}
	}
}
