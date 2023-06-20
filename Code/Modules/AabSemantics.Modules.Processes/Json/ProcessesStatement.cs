using System;
using System.Runtime.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Modules.Processes.Json
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
			: base()
		{ }

		public ProcessesStatement(Statements.ProcessesStatement statement)
			: base(statement)
		{
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
