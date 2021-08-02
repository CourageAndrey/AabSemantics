using Inventor.Core;

namespace Inventor.Client.Commands
{
	public class DeleteConceptCommand : BaseEditCommand
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public DeleteConceptCommand(IConcept concept, ISemanticNetwork semanticNetwork)
			: base(semanticNetwork)
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
