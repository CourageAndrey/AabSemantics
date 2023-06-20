using System.Linq;

using Inventor.Semantics.Modules.WPF.TreeNodes;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Modules.WPF.Commands
{
	public class DeleteConceptCommand : BaseEditCommand
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public int Index
		{ get; private set; }

		#endregion

		public DeleteConceptCommand(IConcept concept, SemanticNetworkNode semanticNetworkNode, IInventorApplication application)
			: base(semanticNetworkNode, application)
		{
			Concept = concept;
		}

		public override void Apply()
		{
			Index = SemanticNetwork.Concepts.IndexOf(Concept);
			SemanticNetwork.Concepts.Remove(Concept);
			var concepts = SemanticNetworkNode.Concepts.Children;
			concepts.Remove(concepts.OfType<ConceptNode>().First(c => c.Concept == Concept));
		}

		public override void Rollback()
		{
			SemanticNetwork.Concepts.Add(Concept);
			SemanticNetworkNode.Concepts.Children.Insert(Index, new ConceptNode(Concept, Application));
		}
	}
}
