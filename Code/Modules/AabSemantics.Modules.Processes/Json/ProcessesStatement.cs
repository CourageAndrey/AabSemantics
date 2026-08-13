using System;
using System.Runtime.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Modules.Processes.Json
{
	/// <summary>JSON surrogate of a <see cref="Statements.ProcessesStatement"/>, storing all three concepts by identifier.</summary>
	[DataContract]
	public class ProcessesStatement : Serialization.Json.Statement<Statements.ProcessesStatement>
	{
		#region Properties

		/// <summary>Identifier of the first process.</summary>
		[DataMember]
		public String ProcessA
		{ get; private set; }

		/// <summary>Identifier of the second process.</summary>
		[DataMember]
		public String ProcessB
		{ get; private set; }

		/// <summary>Identifier of the sequence sign concept.</summary>
		[DataMember]
		public String SequenceSign
		{ get; private set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public ProcessesStatement()
			: base()
		{ }

		/// <summary>Converts a statement into its surrogate.</summary>
		/// <param name="statement">Statement to convert.</param>
		public ProcessesStatement(Statements.ProcessesStatement statement)
			: base(statement)
		{
			ProcessA = statement.ProcessA.ID;
			ProcessB = statement.ProcessB.ID;
			SequenceSign = statement.SequenceSign.ID;
		}

		#endregion

		/// <summary>Restores the statement from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <returns>The restored statement.</returns>
		/// <exception cref="System.ArgumentException">A resolved concept lacks the attribute its role requires.</exception>
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
