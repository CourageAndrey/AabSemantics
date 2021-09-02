using System;
using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[XmlType("Processes")]
	public class ProcessesStatement : Statement
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

		public ProcessesStatement(Statements.ProcessesStatement statement)
		{
			ID = statement.ID;
			ProcessA = statement.ProcessA?.ID;
			ProcessB = statement.ProcessB?.ID;
			SequenceSign = statement.SequenceSign?.ID;
		}

		#endregion

		public override IStatement Save(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.ProcessesStatement(ID, conceptIdResolver.GetConceptById(ProcessA), conceptIdResolver.GetConceptById(ProcessB), conceptIdResolver.GetConceptById(SequenceSign));
		}
	}
}
