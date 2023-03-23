using System;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Json;

namespace Inventor.Semantics.Modules.Classification.Json
{
	[Serializable]
	public class IsStatement : Statement<Statements.IsStatement>
	{
		#region Properties

		public String Ancestor
		{ get; private set; }

		public String Descendant
		{ get; private set; }

		#endregion

		#region Constructors

		public IsStatement()
		{ }

		public IsStatement(Statements.IsStatement statement)
		{
			Ancestor = statement.Ancestor.ID;
			Descendant = statement.Descendant.ID;
		}

		#endregion

		protected override Statements.IsStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.IsStatement(
				ID,
				conceptIdResolver.GetConceptById(Ancestor),
				conceptIdResolver.GetConceptById(Descendant));
		}
	}
}
