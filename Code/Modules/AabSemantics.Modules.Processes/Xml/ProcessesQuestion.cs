using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Processes.Xml
{
	/// <summary>XML surrogate of a <see cref="Questions.ProcessesQuestion"/>.</summary>
	[XmlType]
	public class ProcessesQuestion : Question<Questions.ProcessesQuestion>
	{
		#region Properties

		/// <summary>Identifier of the first process.</summary>
		[XmlElement]
		public String ProcessA
		{ get; set; }

		/// <summary>Identifier of the second process.</summary>
		[XmlElement]
		public String ProcessB
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		public ProcessesQuestion()
		{ }

		/// <summary>Converts a question into its surrogate.</summary>
		/// <param name="question">Question to convert.</param>
		public ProcessesQuestion(Questions.ProcessesQuestion question)
			: base(question)
		{
			ProcessA = question.ProcessA.ID;
			ProcessB = question.ProcessB.ID;
		}

		#endregion

		/// <summary>Restores the question from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Resolves statement identifiers to statements.</param>
		/// <param name="preconditions">Preconditions already restored by the base class.</param>
		/// <returns>The restored question.</returns>
		protected override Questions.ProcessesQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.ProcessesQuestion(
				conceptIdResolver.GetConceptById(ProcessA),
				conceptIdResolver.GetConceptById(ProcessB),
				preconditions);
		}
	}
}
