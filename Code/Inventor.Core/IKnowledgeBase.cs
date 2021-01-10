using System;
using System.Collections.Generic;

using Inventor.Core.Utils;

namespace Inventor.Core
{
	public interface IKnowledgeBase : INamed, IChangeable
	{
		ICollection<IConcept> Concepts
		{ get; }

		ICollection<IStatement> Statements
		{ get; }

		event EventHandler<ItemEventArgs<IConcept>> ConceptAdded;
		event EventHandler<ItemEventArgs<IConcept>> ConceptRemoved;
		event EventHandler<ItemEventArgs<IStatement>> StatementAdded;
		event EventHandler<ItemEventArgs<IStatement>> StatementRemoved;

		IConcept True
		{ get; }

		IConcept False
		{ get; }

		void Save(String fileName);

		FormattedText DescribeRules(ILanguage language);

		FormattedText CheckConsistensy(ILanguage language);
	}
}
