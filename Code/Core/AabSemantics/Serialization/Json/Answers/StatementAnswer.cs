using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using AabSemantics.Text.Primitives;

namespace AabSemantics.Serialization.Json.Answers
{
	[DataContract]
	public class StatementAnswer : Answer
	{
		#region Properties

		[DataMember]
		public Statement Statement
		{ get; set; }

		#endregion

		#region Constructors

		public StatementAnswer()
			: base()
		{ }

		public StatementAnswer(AabSemantics.Answers.StatementAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Statement = Statement.Load(answer.Result);
		}

		#endregion

		public override IAnswer Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			return new AabSemantics.Answers.StatementAnswer(
				Statement.SaveOrReuse(conceptIdResolver, statementIdResolver),
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver))));
		}
	}
}