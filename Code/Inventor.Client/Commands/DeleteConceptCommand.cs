using System.Linq;

using Inventor.Client.TreeNodes;
using Inventor.Core;

namespace Inventor.Client.Commands
{
	public class DeleteConceptCommand : BaseEditCommand
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public DeleteConceptCommand(IConcept concept, SemanticNetworkNode semanticNetworkNode, InventorApplication application)
			: base(semanticNetworkNode, application)
		{
			Concept = concept;
		}

		public override void Apply()
		{
			SemanticNetwork.Concepts.Remove(Concept);
			var concepts = SemanticNetworkNode.Concepts.Children;
			concepts.Remove(concepts.OfType<ConceptNode>().First(c => c.Concept == Concept));
		}

		public override void Rollback()
		{
			SemanticNetwork.Concepts.Add(Concept);
			SemanticNetworkNode.Concepts.Children.Add(new ConceptNode(Concept, Application));
		}
	}
}
