using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using AabSemantics.Serialization;
using AabSemantics.Serialization.Xml;

namespace AabSemantics.Modules.Set.Xml
{
	[XmlType]
	public class HasSignsQuestion : Question<Questions.HasSignsQuestion>
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

		public HasSignsQuestion()
		{ }

		public HasSignsQuestion(Questions.HasSignsQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
			Recursive = question.Recursive;
		}

		#endregion

		protected override Questions.HasSignsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.HasSignsQuestion(
				conceptIdResolver.GetConceptById(Concept),
				Recursive,
				preconditions);
		}
	}
}
