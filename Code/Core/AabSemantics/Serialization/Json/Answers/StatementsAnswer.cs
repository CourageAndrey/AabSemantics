using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using AabSemantics.Text.Primitives;

namespace AabSemantics.Serialization.Json.Answers
{
	/// <summary>JSON surrogate of the <see cref="AabSemantics.Answers.StatementsAnswer"/>.</summary>
	[DataContract]
	public class StatementsAnswer : Answer
	{
		#region Properties

		/// <summary>Surrogates of the statements the answer lists.</summary>
		[DataMember]
		public List<Statement> Statements
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates a surrogate of the "unknown" answer, as required by the JSON serializer.</summary>
		public StatementsAnswer()
			: base()
		{
			Statements = new List<Statement>();
		}

		/// <summary>Converts an answer into its surrogate.</summary>
		/// <param name="answer">Answer to convert.</param>
		/// <param name="language">Language its text is rendered in.</param>
		public StatementsAnswer(AabSemantics.Answers.StatementsAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Statements = answer.Result.Select(statement => Statement.Load(statement)).ToList();
		}

		#endregion

		/// <summary>Restores the answer from the surrogate. Its text comes back as a plain string, not as structured text.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Reuses the network's existing statements where possible.</param>
		/// <returns>The restored answer.</returns>
		public override IAnswer Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			return new AabSemantics.Answers.StatementsAnswer(
				Statements.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver)).ToList(),
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver))));
		}
	}
}