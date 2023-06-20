using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using AabSemantics.Text.Primitives;

namespace AabSemantics.Serialization.Json.Answers
{
	[DataContract]
	public class ConceptAnswer : Answer
	{
		#region Properties

		[DataMember]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public ConceptAnswer()
			: base()
		{ }

		public ConceptAnswer(AabSemantics.Answers.ConceptAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Concept = answer.Result.ID;
		}

		#endregion

		public override IAnswer Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			return new AabSemantics.Answers.ConceptAnswer(
				conceptIdResolver.GetConceptById(Concept),
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver))));
		}
	}
}