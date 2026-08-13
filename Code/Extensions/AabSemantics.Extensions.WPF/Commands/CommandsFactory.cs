using AabSemantics.Extensions.WPF.TreeNodes;
using AabSemantics.Extensions.WPF.ViewModels;

namespace AabSemantics.Extensions.WPF.Commands
{
	/// <summary>Creates the undoable commands behind the UI's edit actions.</summary>
	public interface ICommandsFactory
	{
		/// <summary>Creates the command adding a new knowledge item.</summary>
		/// <param name="viewModel">View model holding the edited values.</param>
		/// <param name="semanticNetworkNode">Tree node representing the network.</param>
		/// <param name="application">The hosting application.</param>
		/// <returns>The command, not yet applied.</returns>
		IEditCommand CreateAddCommand(IKnowledgeViewModel viewModel, SemanticNetworkNode semanticNetworkNode, IInventorApplication application);

		/// <summary>Creates the command applying edits to an existing knowledge item.</summary>
		/// <param name="viewModel">View model holding the edited values.</param>
		/// <param name="semanticNetworkNode">Tree node representing the network.</param>
		/// <param name="application">The hosting application.</param>
		/// <param name="viewModelFactory">Used to snapshot the item's previous state for undo.</param>
		/// <returns>The command, not yet applied.</returns>
		IEditCommand CreateEditCommand(IKnowledgeViewModel viewModel, SemanticNetworkNode semanticNetworkNode, IInventorApplication application, IViewModelFactory viewModelFactory);

		/// <summary>Creates the command deleting the item a tree node stands for.</summary>
		/// <param name="node">Tree node whose item is deleted.</param>
		/// <param name="semanticNetworkNode">Tree node representing the network.</param>
		/// <param name="application">The hosting application.</param>
		/// <returns>The command, not yet applied.</returns>
		IEditCommand CreateDeleteCommand(ExtendedTreeNode node, SemanticNetworkNode semanticNetworkNode, IInventorApplication application);

		/// <summary>Creates the command renaming the knowledge base.</summary>
		/// <param name="name">Edited name.</param>
		/// <param name="semanticNetworkNode">Tree node representing the network.</param>
		/// <param name="application">The hosting application.</param>
		/// <returns>The command, not yet applied.</returns>
		IEditCommand CreateRenameCommand(LocalizedString name, SemanticNetworkNode semanticNetworkNode, IInventorApplication application);
	}

	/// <summary>Default <see cref="ICommandsFactory"/>; override its methods to supply custom commands.</summary>
	public class CommandsFactory : ICommandsFactory
	{
		/// <summary>Creates the command adding a new knowledge item.</summary>
		/// <param name="viewModel">View model holding the edited values.</param>
		/// <param name="semanticNetworkNode">Tree node representing the network.</param>
		/// <param name="application">The hosting application.</param>
		/// <returns>The command, not yet applied.</returns>
		public virtual IEditCommand CreateAddCommand(IKnowledgeViewModel viewModel, SemanticNetworkNode semanticNetworkNode, IInventorApplication application)
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

		/// <summary>Creates the command applying edits to an existing knowledge item.</summary>
		/// <param name="viewModel">View model holding the edited values.</param>
		/// <param name="semanticNetworkNode">Tree node representing the network.</param>
		/// <param name="application">The hosting application.</param>
		/// <param name="viewModelFactory">Used to snapshot the item's previous state for undo.</param>
		/// <returns>The command, not yet applied.</returns>
		public virtual IEditCommand CreateEditCommand(IKnowledgeViewModel viewModel, SemanticNetworkNode semanticNetworkNode, IInventorApplication application, IViewModelFactory viewModelFactory)
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
				var previousVersion = viewModelFactory.CreateStatementByInstance(statementViewModel.BoundStatement, application.CurrentLanguage);
				return new EditStatementCommand(statementViewModel, previousVersion, semanticNetworkNode, application);
			}

			return null;
		}

		/// <summary>Creates the command deleting the item a tree node stands for.</summary>
		/// <param name="node">Tree node whose item is deleted.</param>
		/// <param name="semanticNetworkNode">Tree node representing the network.</param>
		/// <param name="application">The hosting application.</param>
		/// <returns>The command, not yet applied.</returns>
		public virtual IEditCommand CreateDeleteCommand(ExtendedTreeNode node, SemanticNetworkNode semanticNetworkNode, IInventorApplication application)
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

		/// <summary>Creates the command renaming the knowledge base.</summary>
		/// <param name="name">Edited name.</param>
		/// <param name="semanticNetworkNode">Tree node representing the network.</param>
		/// <param name="application">The hosting application.</param>
		/// <returns>The command, not yet applied.</returns>
		public virtual IEditCommand CreateRenameCommand(LocalizedString name, SemanticNetworkNode semanticNetworkNode, IInventorApplication application)
		{
			return new RenameSemanticNetworkCommand(semanticNetworkNode, name, application);
		}
	}
}