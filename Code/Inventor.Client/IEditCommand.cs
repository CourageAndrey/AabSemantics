using Inventor.Client.Commands;
using Inventor.Client.TreeNodes;
using Inventor.Client.ViewModels;
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

	public static class CommandsHelper
	{
		public static IEditCommand CreateAddCommand(this IKnowledgeViewModel viewModel, SemanticNetworkNode semanticNetworkNode, InventorApplication application)
		{
			var conceptViewModel = viewModel as Concept;
			if (conceptViewModel != null)
			{
				return new AddConceptCommand(conceptViewModel, semanticNetworkNode, application);
			}

			var statementViewModel = viewModel as StatementViewModel;
			if (statementViewModel != null)
			{
				return new AddStatementCommand(statementViewModel, semanticNetworkNode, application);
			}

			return null;
		}

		public static IEditCommand CreateEditCommand(this IKnowledgeViewModel viewModel, SemanticNetworkNode semanticNetworkNode, InventorApplication application)
		{
			var conceptViewModel = viewModel as Concept;
			if (conceptViewModel != null)
			{
				var previousVersion = new Concept(conceptViewModel.BoundObject);
				return new EditConceptCommand(conceptViewModel, previousVersion, semanticNetworkNode, application);
			}

			var statementViewModel = viewModel as StatementViewModel;
			if (statementViewModel != null)
			{
				var previousVersion = ViewModelFactory.CreateStatementByInstance(statementViewModel.BoundStatement, application.CurrentLanguage);
				return new EditStatementCommand(statementViewModel, previousVersion, semanticNetworkNode, application);
			}

			return null;
		}

		public static IEditCommand CreateDeleteCommand(this ExtendedTreeNode node, SemanticNetworkNode semanticNetworkNode, InventorApplication application)
		{
			var conceptNode = node as ConceptNode;
			if (conceptNode != null)
			{
				return new DeleteConceptCommand(conceptNode.Concept, semanticNetworkNode, application);
			}

			var statementNode = node as StatementNode;
			if (statementNode != null)
			{
				return new DeleteStatementCommand(statementNode.Statement, semanticNetworkNode, application);
			}

			return null;
		}

		public static IEditCommand CreateRenameCommand(this LocalizedString name, SemanticNetworkNode semanticNetworkNode, InventorApplication application)
		{
			return new RenameSemanticNetworkCommand(semanticNetworkNode, name, application);
		}
	}
}
