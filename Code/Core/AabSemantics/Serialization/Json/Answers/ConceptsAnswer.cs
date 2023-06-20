using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using AabSemantics.Text.Primitives;

namespace AabSemantics.Serialization.Json.Answers
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
		{
			Concepts = new List<String>();
		}

		public ConceptsAnswer(AabSemantics.Answers.ConceptsAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Concepts = answer.Result.Select(concept => concept.ID).ToList();
		}

		#endregion

		public override IAnswer Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			return new AabSemantics.Answers.ConceptsAnswer(
				Concepts.Select(concept => conceptIdResolver.GetConceptById(concept)).ToList(),
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver))));
		}
	}
}