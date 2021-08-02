using Inventor.Client.Commands;
using Inventor.Client.TreeNodes;
using Inventor.Core;

namespace Inventor.Client
{
	public interface IEditCommand
	{
		ISemanticNetwork SemanticNetwork
		{ get; }

		void Apply();

		void Rollback();
	}

	public abstract class BaseEditCommand : IEditCommand
	{
		public ISemanticNetwork SemanticNetwork
		{ get; }

		protected BaseEditCommand(ISemanticNetwork semanticNetwork)
		{
			SemanticNetwork = semanticNetwork;
		}

		public abstract void Apply();

		public abstract void Rollback();
	}

	public static class CommandsHelper
	{
		public static IEditCommand CreateAddCommand(this IKnowledgeViewModel viewModel, ISemanticNetwork semanticNetwork)
		{
			var conceptViewModel = viewModel as ViewModels.Concept;
			if (conceptViewModel != null)
			{
				return new AddConceptCommand(conceptViewModel, semanticNetwork);
			}

			var statementViewModel = viewModel as StatementViewModel;
			if (statementViewModel != null)
			{
				return new AddStatementCommand(statementViewModel, semanticNetwork);
			}

			return null;
		}

		public static IEditCommand CreateEditCommand(this IKnowledgeViewModel viewModel, ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var conceptViewModel = viewModel as ViewModels.Concept;
			if (conceptViewModel != null)
			{
				return new EditConceptCommand(conceptViewModel, semanticNetwork);
			}

			var statementViewModel = viewModel as StatementViewModel;
			if (statementViewModel != null)
			{
				return new EditStatementCommand(language, statementViewModel, semanticNetwork);
			}

			return null;
		}

		public static IEditCommand CreateDeleteCommand(this ExtendedTreeNode node, ISemanticNetwork semanticNetwork)
		{
			var conceptNode = node as ConceptNode;
			if (conceptNode != null)
			{
				return new DeleteConceptCommand(conceptNode.Concept, semanticNetwork);
			}

			var statementNode = node as StatementNode;
			if (statementNode != null)
			{
				return new DeleteStatementCommand(statementNode.Statement, semanticNetwork);
			}

			return null;
		}
	}
}
