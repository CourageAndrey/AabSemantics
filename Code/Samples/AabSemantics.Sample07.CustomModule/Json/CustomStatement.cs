using System.Runtime.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Sample07.CustomModule.Json
{
	[DataContract]
	public class CustomStatement : AabSemantics.Serialization.Json.Statement<Sample07.CustomModule.CustomStatement>
	{
		#region Properties

		[DataMember]
		public string Concept1
		{ get; set; }

		[DataMember]
		public string Concept2
		{ get; set; }

		#endregion

		#region Constructors

		public CustomStatement()
			: base()
		{ }

		public CustomStatement(Sample07.CustomModule.CustomStatement statement)
			: base(statement)
		{
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
