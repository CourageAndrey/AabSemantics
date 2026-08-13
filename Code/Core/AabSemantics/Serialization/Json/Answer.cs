using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

using AabSemantics.Metadata;
using AabSemantics.Serialization.Json.Answers;
using AabSemantics.Text.Primitives;

namespace AabSemantics.Serialization.Json
{
	/// <summary>
	/// JSON surrogate of an answer, and the base of the typed answer surrogates. Its static
	/// constructor calls <see cref="RefreshMetadata"/> so explanations stay polymorphic.
	/// </summary>
	[DataContract]
	public class Answer
	{
		#region Properties

		/// <summary>
		/// The answer's text, already rendered to a plain string. Structure and language are lost,
		/// so a round trip does not restore the original <see cref="IText"/> tree.
		/// </summary>
		[DataMember]
		public String Description
		{ get; set; }

		/// <summary>Surrogates of the statements the answer was derived from.</summary>
		[DataMember]
		public List<Statement> Explanation
		{ get; set; }

		/// <summary>Whether the answer means "unknown".</summary>
		[DataMember]
		public Boolean IsEmpty
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the JSON serializer.</summary>
		public Answer()
			: this(String.Empty, new List<Statement>(), true)
		{ }

		/// <summary>Creates a surrogate from its parts.</summary>
		/// <param name="description">The answer's text as a plain string.</param>
		/// <param name="explanation">Surrogates of the supporting statements.</param>
		/// <param name="isEmpty">Whether the answer means "unknown".</param>
		public Answer(String description, List<Statement> explanation, Boolean isEmpty)
		{
			Description = description;
			Explanation = explanation;
			IsEmpty = isEmpty;
		}

		/// <summary>Converts an answer into its surrogate, rendering its text in the given language.</summary>
		/// <param name="answer">Answer to convert.</param>
		/// <param name="language">Language its text is rendered in.</param>
		public Answer(IAnswer answer, ILanguage language)
			: this(
				TextRenders.PlainString.Render(answer.Description, language).ToString(),
				answer.Explanation.Statements.Select(statement => Statement.Load(statement)).ToList(),
				answer.IsEmpty)
		{ }

		#endregion

		/// <summary>Converts an answer into the surrogate registered for its type.</summary>
		/// <param name="answer">Answer to convert.</param>
		/// <param name="language">Language its text is rendered in.</param>
		/// <returns>The surrogate, ready to be serialized.</returns>
		/// <exception cref="NotSupportedException">The answer's type is not registered.</exception>
		public static Answer Load(IAnswer answer, ILanguage language)
		{
			var definition = Repositories.Answers.Definitions.GetSuitable(answer);
			return definition.GetSerializationSettings<AnswerJsonSerializationSettings>().GetJson(answer, language);
		}

		/// <summary>Restores the answer from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Reuses the network's existing statements where possible.</param>
		/// <returns>The restored answer, with its text as a plain string.</returns>
		public virtual IAnswer Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			return new AabSemantics.Answers.Answer(
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver))),
				IsEmpty);
		}

		static Answer()
		{
			RefreshMetadata();
		}

		/// <summary>
		/// Rebuilds the JSON serializers of every answer surrogate so they know the currently
		/// registered statement types. Call it again after registering further statement types,
		/// otherwise those cannot appear in a serialized explanation.
		/// </summary>
		public static void RefreshMetadata()
		{
			var statementTypes = Repositories.Statements.GetJsonTypes();
			foreach (var answerType in new[]
			{
				typeof(Answer),
				typeof(BooleanAnswer),
				typeof(ConceptAnswer),
				typeof(ConceptsAnswer),
				typeof(StatementAnswer),
				typeof(StatementsAnswer),
			})
			{
				var serializer = new DataContractJsonSerializer(
					answerType,
					statementTypes);
				answerType.DefineCustomJsonSerializer(serializer);
			}
		}
	}
}
