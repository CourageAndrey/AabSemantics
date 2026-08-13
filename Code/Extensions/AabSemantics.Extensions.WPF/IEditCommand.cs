using AabSemantics.Extensions.WPF.TreeNodes;

namespace AabSemantics.Extensions.WPF
{
	/// <summary>A reversible edit, so the UI can offer undo and redo.</summary>
	public interface IEditCommand
	{
		/// <summary>Performs the edit.</summary>
		void Apply();

		/// <summary>Reverses the edit, restoring the previous state.</summary>
		void Rollback();
	}

	/// <summary>Base edit command, holding the network node and the hosting application.</summary>
	public abstract class BaseEditCommand : IEditCommand
	{
		/// <summary>Tree node representing the edited network.</summary>
		public SemanticNetworkNode SemanticNetworkNode
		{ get; }

		/// <summary>The network being edited.</summary>
		protected ISemanticNetwork SemanticNetwork
		{ get { return SemanticNetworkNode.SemanticNetwork; } }

		/// <summary>The hosting application.</summary>
		protected readonly IInventorApplication Application;

		/// <summary>Initializes the command.</summary>
		/// <param name="semanticNetworkNode">Tree node representing the edited network.</param>
		/// <param name="application">The hosting application.</param>
		protected BaseEditCommand(SemanticNetworkNode semanticNetworkNode, IInventorApplication application)
		{
			SemanticNetworkNode = semanticNetworkNode;
			Application = application;
		}

		/// <summary>Performs the edit.</summary>
		public abstract void Apply();

		/// <summary>Reverses the edit, restoring the previous state.</summary>
		public abstract void Rollback();
	}
}
