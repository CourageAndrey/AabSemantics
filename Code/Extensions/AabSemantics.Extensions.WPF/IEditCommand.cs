using AabSemantics.Extensions.WPF.TreeNodes;

namespace AabSemantics.Extensions.WPF
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

		protected readonly IInventorApplication Application;

		protected BaseEditCommand(SemanticNetworkNode semanticNetworkNode, IInventorApplication application)
		{
			SemanticNetworkNode = semanticNetworkNode;
			Application = application;
		}

		public abstract void Apply();

		public abstract void Rollback();
	}
}
