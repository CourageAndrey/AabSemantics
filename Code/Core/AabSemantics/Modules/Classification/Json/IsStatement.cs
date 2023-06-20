using System;
using System.Runtime.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Json;

namespace AabSemantics.Modules.Classification.Json
{
	[DataContract]
	public class IsStatement : Statement<Statements.IsStatement>
	{
		#region Properties

		[DataMember]
		public String Ancestor
		{ get; set; }

		[DataMember]
		public String Descendant
		{ get; set; }

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
