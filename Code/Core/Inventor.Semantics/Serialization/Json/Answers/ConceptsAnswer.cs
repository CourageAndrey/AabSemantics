using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Inventor.Semantics.Text.Primitives;

namespace Inventor.Semantics.Serialization.Json.Answers
{
	[DataContract]
	public class ConceptsAnswer : Answer
	{
		#region Properties

		[DataMember]
		public List<String> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		public ConceptsAnswer()
			: base()
		{ }

		public ConceptsAnswer(Semantics.Answers.ConceptsAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Concepts = answer.Result.Select(concept => concept.ID).ToList();
		}

		#endregion

		public override IAnswer Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			return new Semantics.Answers.ConceptsAnswer(
				Concepts.Select(concept => conceptIdResolver.GetConceptById(concept)).ToList(),
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver))));
		}
	}
}