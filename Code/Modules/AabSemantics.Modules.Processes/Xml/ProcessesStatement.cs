using System;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Processes.Xml
{
	[XmlType("Processes")]
	public class ProcessesStatement : Statement<Statements.ProcessesStatement>
	{
		#region Properties

		[XmlAttribute]
		public String ProcessA
		{ get; set; }

		[XmlAttribute]
		public String ProcessB
		{ get; set; }

		[XmlAttribute]
		public String SequenceSign
		{ get; set; }

		#endregion

		#region Constructors

		public ProcessesStatement()
		{ }

		public ProcessesStatement(Processes.Statements.ProcessesStatement statement)
		{
			ID = statement.ID;
			ProcessA = statement.ProcessA?.ID;
			ProcessB = statement.ProcessB?.ID;
			SequenceSign = statement.SequenceSign?.ID;
		}

		#endregion

		protected override Statements.ProcessesStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.ProcessesStatement(ID, conceptIdResolver.GetConceptById(ProcessA), conceptIdResolver.GetConceptById(ProcessB), conceptIdResolver.GetConceptById(SequenceSign));
		}
	}
}
