using System;
using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IKnowledgeBase : INamed, IChangeable
	{
		ICollection<Concept> Concepts
		{ get; }

		ICollection<Statement> Statements
		{ get; }

		event EventHandler<ItemEventArgs<Concept>> ConceptAdded;
		event EventHandler<ItemEventArgs<Concept>> ConceptRemoved;
		event EventHandler<ItemEventArgs<Statement>> StatementAdded;
		event EventHandler<ItemEventArgs<Statement>> StatementRemoved;
	}
}
