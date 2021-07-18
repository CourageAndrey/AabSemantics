using System;
using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[XmlType]
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

		public ProcessesStatement(Statements.ProcessesStatement statement, LoadIdResolver conceptIdResolver)
		{
			ProcessA = conceptIdResolver.GetConceptId(statement.ProcessA);
			ProcessB = conceptIdResolver.GetConceptId(statement.ProcessB);
			SequenceSign = conceptIdResolver.GetConceptId(statement.SequenceSign);
		}

		#endregion

		public override IStatement Save(SaveIdResolver conceptIdResolver)
		{
			return new Statements.ProcessesStatement(conceptIdResolver.GetConceptById(ProcessA), conceptIdResolver.GetConceptById(ProcessB), conceptIdResolver.GetConceptById(SequenceSign));
		}
	}
}
