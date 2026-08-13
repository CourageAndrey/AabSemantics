using System;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Processes.Xml
{
	/// <summary>XML surrogate of a <see cref="Statements.ProcessesStatement"/>, storing all three concepts by identifier.</summary>
	[XmlType("Processes")]
	public class ProcessesStatement : Statement<Statements.ProcessesStatement>
	{
		#region Properties

		/// <summary>Identifier of the first process.</summary>
		[XmlAttribute]
		public String ProcessA
		{ get; set; }

		/// <summary>Identifier of the second process.</summary>
		[XmlAttribute]
		public String ProcessB
		{ get; set; }

		/// <summary>Identifier of the sequence sign concept.</summary>
		[XmlAttribute]
		public String SequenceSign
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public ProcessesStatement()
		{ }

		/// <summary>Converts a statement into its surrogate.</summary>
		/// <param name="statement">Statement to convert.</param>
		public ProcessesStatement(Statements.ProcessesStatement statement)
			: base(statement)
		{
			ProcessA = statement.ProcessA?.ID;
			ProcessB = statement.ProcessB?.ID;
			SequenceSign = statement.SequenceSign?.ID;
		}

		#endregion

		/// <summary>Restores the statement from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <returns>The restored statement.</returns>
		/// <exception cref="System.ArgumentException">A resolved concept lacks the attribute its role requires.</exception>
		protected override Statements.ProcessesStatement SaveImplementation(ConceptIdResolver conceptIdResolver)
		{
			return new Statements.ProcessesStatement(ID, conceptIdResolver.GetConceptById(ProcessA), conceptIdResolver.GetConceptById(ProcessB), conceptIdResolver.GetConceptById(SequenceSign));
		}
	}
}
