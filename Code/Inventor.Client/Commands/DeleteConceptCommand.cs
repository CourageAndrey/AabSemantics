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

		public DeleteConceptCommand(IConcept concept, SemanticNetworkNode semanticNetworkNode)
			: base(semanticNetworkNode)
		{
			Concept = concept;
		}

		public override void Apply()
		{
			SemanticNetwork.Concepts.Remove(Concept);
		}

		public override void Rollback()
		{
			SemanticNetwork.Concepts.Add(Concept);
		}
	}
}
