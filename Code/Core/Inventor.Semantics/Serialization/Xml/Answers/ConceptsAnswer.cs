using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Inventor.Semantics.Localization;

namespace Inventor.Semantics.Serialization.Xml.Answers
{
	[XmlType]
	public class ConceptsAnswer : Answer
	{
		#region Properties

		[XmlArray(nameof(Concepts))]
		[XmlArrayItem(nameof(Concept))]
		public List<Concept> Concepts
		{ get; }

		#endregion

		#region Constructors

		public ConceptsAnswer()
			: base(Semantics.Answers.Answer.CreateUnknown(), Language.Default)
		{
			Concepts = new List<Concept>();
		}

		public ConceptsAnswer(Semantics.Answers.ConceptsAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Concepts = answer.Result.Select(concept => new Concept(concept)).ToList();
		}

		#endregion
	}
}