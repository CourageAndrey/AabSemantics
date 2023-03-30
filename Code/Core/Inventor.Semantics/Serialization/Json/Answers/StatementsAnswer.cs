using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Inventor.Semantics.Text.Primitives;

namespace Inventor.Semantics.Serialization.Json.Answers
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
		{ }

		public StatementsAnswer(Semantics.Answers.StatementsAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Statements = answer.Result.Select(statement => Statement.Load(statement)).ToList();
		}

		#endregion

		public override IAnswer Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			return new Semantics.Answers.StatementsAnswer(
				Statements.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver)).ToList(),
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver))));
		}
	}
}