using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Text.Primitives;

namespace Inventor.Semantics.Serialization.Xml.Answers
{
	[XmlType]
	public class ConceptsAnswer : Answer
	{
		#region Properties

		[XmlArray(nameof(Concepts))]
		[XmlArrayItem(nameof(Concept))]
		public List<String> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		public ConceptsAnswer()
			: base(Semantics.Answers.Answer.CreateUnknown(), Language.Default)
		{
			Concepts = new List<String>();
		}

		public ConceptsAnswer(Semantics.Answers.ConceptsAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Concepts = answer.Result.Select(concept => concept.ID).ToList();
		}

		#endregion

		public override IAnswer Save(ConceptIdResolver conceptIdResolver)
		{
			return new Semantics.Answers.ConceptsAnswer(
				Concepts.Select(concept => conceptIdResolver.GetConceptById(concept)).ToList(),
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.Save(conceptIdResolver))));
		}
	}
}