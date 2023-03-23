using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Inventor.Semantics.Serialization.Json.Answers
{
	[DataContract]
	public class ConceptsAnswer : Answer
	{
		#region Properties

		[DataMember]
		public List<Concept> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		public ConceptsAnswer()
			: base()
		{ }

		public ConceptsAnswer(Semantics.Answers.ConceptsAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Concepts = answer.Result.Select(concept => new Concept(concept)).ToList();
		}

		#endregion
	}
}