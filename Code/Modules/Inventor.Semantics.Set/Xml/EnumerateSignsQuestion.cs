using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Set.Xml
{
	[XmlType]
	public class EnumerateSignsQuestion : Question<Questions.EnumerateSignsQuestion>
	{
		#region Properties

		[XmlElement]
		public String Concept
		{ get; }

		[XmlElement]
		public Boolean Recursive
		{ get; }

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

		protected override Questions.EnumerateSignsQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.EnumerateSignsQuestion(
				conceptIdResolver.GetConceptById(Concept),
				Recursive,
				preconditions);
		}
	}
}
