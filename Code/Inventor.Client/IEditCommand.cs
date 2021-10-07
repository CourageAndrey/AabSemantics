using Inventor.Client.TreeNodes;
using Inventor.Core;

namespace Inventor.Client
{
	public interface IEditCommand
	{
		void Apply();

		void Rollback();
	}

	public abstract class BaseEditCommand : IEditCommand
	{
		public SemanticNetworkNode SemanticNetworkNode
		{ get; }

		protected ISemanticNetwork SemanticNetwork
		{ get { return SemanticNetworkNode.SemanticNetwork; } }

		protected readonly InventorApplication Application;

		protected BaseEditCommand(SemanticNetworkNode semanticNetworkNode, InventorApplication application)
		{
			SemanticNetworkNode = semanticNetworkNode;
			Application = application;
		}

		public abstract void Apply();

		public abstract void Rollback();
	}
}
