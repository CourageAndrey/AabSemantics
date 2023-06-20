using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Sample07.CustomModule.Xml
{
	[XmlType]
	public class CustomQuestion : Question<Sample07.CustomModule.CustomQuestion>
	{
		#region Properties

		[XmlElement]
		public String Concept1
		{ get; set; }

		[XmlElement]
		public String Concept2
		{ get; set; }

		#endregion

		#region Constructors

		public CustomQuestion()
		{ }

		public CustomQuestion(Sample07.CustomModule.CustomQuestion question)
			: base(question)
		{
			Concept1 = question.Concept1.ID;
			Concept2 = question.Concept2.ID;
		}

		#endregion

		protected override Sample07.CustomModule.CustomQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Sample07.CustomModule.CustomQuestion(
				conceptIdResolver.GetConceptById(Concept1),
				conceptIdResolver.GetConceptById(Concept2),
				preconditions);
		}
	}
}
