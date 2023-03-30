using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Inventor.Semantics.Text.Primitives;

namespace Inventor.Semantics.Serialization.Json.Answers
{
	[DataContract]
	public class BooleanAnswer : Answer
	{
		#region Properties

		[DataMember]
		public Boolean Result
		{ get; set; }

		#endregion

		#region Constructors

		public BooleanAnswer()
			: base()
		{ }

		public BooleanAnswer(Semantics.Answers.BooleanAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Result = answer.Result;
		}

		#endregion

		public override IAnswer Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			return new Semantics.Answers.BooleanAnswer(
				Result,
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver))));
		}
	}
}