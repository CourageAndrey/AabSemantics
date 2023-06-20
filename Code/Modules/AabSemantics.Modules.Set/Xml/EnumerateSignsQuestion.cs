using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Set.Xml
{
	[XmlType]
	public class EnumerateSignsQuestion : Question<Questions.EnumerateSignsQuestion>
	{
		#region Properties

		[XmlElement]
		public String Concept
		{ get; set; }

		[XmlElement]
		public System.Boolean Recursive
		{ get; set; }

		#endregion

		#region Constructors

		public EnumerateSignsQuestion()
		{ }

		public EnumerateSignsQuestion(Questions.EnumerateSignsQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
			Recursive = question.Recursive;
		}

		#endregion

		protected override Questions.EnumerateSignsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.EnumerateSignsQuestion(
				conceptIdResolver.GetConceptById(Concept),
				Recursive,
				preconditions);
		}
	}
}
