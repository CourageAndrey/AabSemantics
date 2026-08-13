using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using AabSemantics.Localization;
using AabSemantics.Text.Primitives;

namespace AabSemantics.Serialization.Xml.Answers
{
	/// <summary>XML surrogate of the <see cref="AabSemantics.Answers.ConceptAnswer"/>.</summary>
	[XmlType]
	public class ConceptAnswer : Answer
	{
		#region Properties

		/// <summary>Identifier of the concept the answer names.</summary>
		[XmlElement]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates a surrogate of the "unknown" answer, as required by the XML serializer.</summary>
		public ConceptAnswer()
			: base(AabSemantics.Answers.Answer.CreateUnknown(), Language.Default)
		{ }

		/// <summary>Converts an answer into its surrogate.</summary>
		/// <param name="answer">Answer to convert.</param>
		/// <param name="language">Language its text is rendered in.</param>
		public ConceptAnswer(AabSemantics.Answers.ConceptAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Concept = answer.Result.ID;
		}

		#endregion

		/// <summary>Restores the answer from the surrogate. Its text comes back as a plain string, not as structured text.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Reuses the network's existing statements where possible.</param>
		/// <returns>The restored answer.</returns>
		public override IAnswer Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			return new AabSemantics.Answers.ConceptAnswer(
				conceptIdResolver.GetConceptById(Concept),
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver))));
		}
	}
}