using System;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Processes.Json
{
	[DataContract]
	public class ProcessesStatement : Serialization.Json.Statement<Statements.ProcessesStatement>
	{
		#region Properties

		[DataMember]
		public String ProcessA
		{ get; private set; }

		[DataMember]
		public String ProcessB
		{ get; private set; }

		[DataMember]
		public String SequenceSign
		{ get; private set; }

		#endregion

		#region Constructors

		public ProcessesStatement()
		{ }

		public ProcessesStatement(Statements.ProcessesStatement statement)
		{
			ID = statement.ID;
			ProcessA = statement.ProcessA.ID;
			ProcessB = statement.ProcessB.ID;
			SequenceSign = statement.SequenceSign.ID;
		}

		#endregion

		protected override Statements.ProcessesStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.ProcessesStatement(
				ID,
				conceptIdResolver.GetConceptById(ProcessA),
				conceptIdResolver.GetConceptById(ProcessB),
				conceptIdResolver.GetConceptById(SequenceSign));
		}
	}
}
