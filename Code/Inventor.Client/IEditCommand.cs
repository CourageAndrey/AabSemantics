using Inventor.Client.Commands;
using Inventor.Client.TreeNodes;
using Inventor.Client.ViewModels;
using Inventor.Core;

namespace Inventor.Client
{
	public interface IEditCommand
	{
		SemanticNetworkNode SemanticNetworkNode
		{ get; }

		void Apply();

		void Rollback();
	}

	public abstract class BaseEditCommand : IEditCommand
	{
		public SemanticNetworkNode SemanticNetworkNode
		{ get; }

		protected ISemanticNetwork SemanticNetwork
		{ get { return SemanticNetworkNode.SemanticNetwork; } }

		protected BaseEditCommand(SemanticNetworkNode semanticNetworkNode)
		{
			SemanticNetworkNode = semanticNetworkNode;
		}

		public abstract void Apply();

		public abstract void Rollback();
	}

	public static class CommandsHelper
	{
		public static IEditCommand CreateAddCommand(this IKnowledgeViewModel viewModel, SemanticNetworkNode semanticNetworkNode)
		{
			var conceptViewModel = viewModel as ViewModels.Concept;
			if (conceptViewModel != null)
			{
				return new AddConceptCommand(conceptViewModel, semanticNetworkNode);
			}

			var statementViewModel = viewModel as StatementViewModel;
			if (statementViewModel != null)
			{
				return new AddStatementCommand(statementViewModel, semanticNetworkNode);
			}

			return null;
		}

		public static IEditCommand CreateEditCommand(this IKnowledgeViewModel viewModel, SemanticNetworkNode semanticNetworkNode, ILanguage language)
		{
			var conceptViewModel = viewModel as ViewModels.Concept;
			if (conceptViewModel != null)
			{
				return new EditConceptCommand(conceptViewModel, semanticNetworkNode);
			}

			var statementViewModel = viewModel as StatementViewModel;
			if (statementViewModel != null)
			{
				return new EditStatementCommand(language, statementViewModel, semanticNetworkNode);
			}

			return null;
		}

		public static IEditCommand CreateDeleteCommand(this ExtendedTreeNode node, SemanticNetworkNode semanticNetworkNode)
		{
			var conceptNode = node as ConceptNode;
			if (conceptNode != null)
			{
				return new DeleteConceptCommand(conceptNode.Concept, semanticNetworkNode);
			}

			var statementNode = node as StatementNode;
			if (statementNode != null)
			{
				return new DeleteStatementCommand(statementNode.Statement, semanticNetworkNode);
			}

			return null;
		}

		public static IEditCommand CreateRenameCommand(this LocalizedString name, SemanticNetworkNode semanticNetworkNode)
		{
			return new RenameSemanticNetworkCommand(semanticNetworkNode, name);
		}
	}
}
