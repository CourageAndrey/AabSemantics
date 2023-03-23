using System;

using Inventor.Semantics.Serialization;

namespace Samples.Semantics.Sample07.CustomModule.Json
{
	[Serializable]
	public class CustomStatement : Inventor.Semantics.Serialization.Json.Statement<Sample07.CustomModule.CustomStatement>
	{
		#region Properties

		public string Concept1
		{ get; set; }

		public string Concept2
		{ get; set; }

		#endregion

		#region Constructors

		public CustomStatement()
		{ }

		public CustomStatement(Sample07.CustomModule.CustomStatement statement)
		{
			ID = statement.ID;
			Concept1 = statement.Concept1?.ID;
			Concept2 = statement.Concept2?.ID;
		}

		#endregion

		protected override Sample07.CustomModule.CustomStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Sample07.CustomModule.CustomStatement(ID, conceptIdResolver.GetConceptById(Concept1), conceptIdResolver.GetConceptById(Concept2));
		}
	}
}
