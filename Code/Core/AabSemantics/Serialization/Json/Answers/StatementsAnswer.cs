using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using AabSemantics.Text.Primitives;

namespace AabSemantics.Serialization.Json.Answers
{
	[DataContract]
	public class StatementsAnswer : Answer
	{
		#region Properties

		[DataMember]
		public List<Statement> Statements
		{ get; set; }

		#endregion

		#region Constructors

		public StatementsAnswer()
			: base()
		{
			Statements = new List<Statement>();
		}

		public StatementsAnswer(AabSemantics.Answers.StatementsAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Statements = answer.Result.Select(statement => Statement.Load(statement)).ToList();
		}

		#endregion

		public override IAnswer Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			return new AabSemantics.Answers.StatementsAnswer(
				Statements.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver)).ToList(),
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver))));
		}
	}
}