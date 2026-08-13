using System.Linq;

using AabSemantics.Extensions.WPF.TreeNodes;
using AabSemantics.Utils;

namespace AabSemantics.Extensions.WPF.Commands
{
	/// <summary>Undoable command that deletes a concept.</summary>
	public class DeleteConceptCommand : BaseEditCommand
	{
		#region Properties

		/// <summary>The concept in question.</summary>
		public IConcept Concept
		{ get; }

		/// <summary>Position the item occupied before deletion, so undo can restore the order.</summary>
		public int Index
		{ get; private set; }

		#endregion

		/// <summary>Creates the command.</summary>
		/// <param name="concept">The concept in question.</param>
		/// <param name="semanticNetworkNode">Tree node representing the network.</param>
		/// <param name="application">The hosting application.</param>
		public DeleteConceptCommand(IConcept concept, SemanticNetworkNode semanticNetworkNode, IInventorApplication application)
			: base(semanticNetworkNode, application)
		{
			Concept = concept;
		}

		/// <summary>Performs the edit.</summary>
		public override void Apply()
		{
			Index = SemanticNetwork.Concepts.IndexOf(Concept);
			SemanticNetwork.Concepts.Remove(Concept);
			var concepts = SemanticNetworkNode.Concepts.Children;
			concepts.Remove(concepts.OfType<ConceptNode>().First(c => c.Concept == Concept));
		}

		/// <summary>Reverses the edit, restoring the previous state.</summary>
		public override void Rollback()
		{
			SemanticNetwork.Concepts.Add(Concept);
			SemanticNetworkNode.Concepts.Children.Insert(Index, new ConceptNode(Concept, Application));
		}
	}
}
