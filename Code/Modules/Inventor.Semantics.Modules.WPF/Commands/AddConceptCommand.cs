using System.Linq;

using Inventor.Semantics.WPF.TreeNodes;

namespace Inventor.Semantics.WPF.Commands
{
	public class AddConceptCommand : BaseEditCommand
	{
		#region Properties

		public ViewModels.Concept ViewModel
		{ get; }

		public IConcept NewItem
		{ get; private set; }

		#endregion

		public AddConceptCommand(ViewModels.Concept viewModel, SemanticNetworkNode semanticNetworkNode, IInventorApplication application)
			: base(semanticNetworkNode, application)
		{
			ViewModel = viewModel;
		}

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

		public override void Rollback()
		{
			SemanticNetwork.Concepts.Remove(NewItem);
			var concepts = SemanticNetworkNode.Concepts.Children;
			concepts.Remove(concepts.OfType<ConceptNode>().First(c => c.Concept == NewItem));
		}
	}
}
