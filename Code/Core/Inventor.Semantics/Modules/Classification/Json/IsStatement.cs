using System;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Json;

namespace Inventor.Semantics.Modules.Classification.Json
{
	[DataContract]
	public class IsStatement : Statement<Statements.IsStatement>
	{
		#region Properties

		[DataMember]
		public String Ancestor
		{ get; private set; }

		[DataMember]
		public String Descendant
		{ get; private set; }

		#endregion

		#region Constructors

		public IsStatement()
			: base()
		{ }

		public IsStatement(Statements.IsStatement statement)
			: base(statement)
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
