using System.Linq;

using AabSemantics.Extensions.WPF.TreeNodes;

namespace AabSemantics.Extensions.WPF.Commands
{
	/// <summary>Undoable command that adds a concept.</summary>
	public class AddConceptCommand : BaseEditCommand
	{
		#region Properties

		/// <summary>View model holding the edited values.</summary>
		public ViewModels.Concept ViewModel
		{ get; }

		/// <summary>The item created by this command.</summary>
		public IConcept NewItem
		{ get; private set; }

		#endregion

		/// <summary>Creates the command.</summary>
		/// <param name="viewModel">View model holding the edited values.</param>
		/// <param name="semanticNetworkNode">Tree node representing the network.</param>
		/// <param name="application">The hosting application.</param>
		public AddConceptCommand(ViewModels.Concept viewModel, SemanticNetworkNode semanticNetworkNode, IInventorApplication application)
			: base(semanticNetworkNode, application)
		{
			ViewModel = viewModel;
		}

		/// <summary>Performs the edit.</summary>
		public override void Apply()
		{
			if (NewItem == null)
			{
				NewItem = (IConcept) ViewModel.ApplyCreate(SemanticNetworkNode.SemanticNetwork);
			}
			else
			{
				SemanticNetwork.Concepts.Add(NewItem);
			}
			SemanticNetworkNode.Concepts.Children.Add(new ConceptNode(NewItem, Application));
		}

		/// <summary>Reverses the edit, restoring the previous state.</summary>
		public override void Rollback()
		{
			SemanticNetwork.Concepts.Remove(NewItem);
			var concepts = SemanticNetworkNode.Concepts.Children;
			concepts.Remove(concepts.OfType<ConceptNode>().First(c => c.Concept == NewItem));
		}
	}
}
