using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Serialization.Xml.Answers;
using AabSemantics.Text.Primitives;

namespace AabSemantics.Serialization.Xml
{
	/// <summary>
	/// XML surrogate of an answer, and the base of the typed answer surrogates. The static
	/// constructor teaches the serializer every registered statement type, so explanations stay
	/// polymorphic.
	/// </summary>
	[XmlType]
	[XmlInclude(typeof(BooleanAnswer))]
	[XmlInclude(typeof(ConceptAnswer))]
	[XmlInclude(typeof(ConceptsAnswer))]
	[XmlInclude(typeof(StatementAnswer))]
	[XmlInclude(typeof(StatementsAnswer))]
	public class Answer
	{
		#region Properties

		/// <summary>
		/// The answer's text, already rendered to a plain string. Structure and language are lost,
		/// so a round trip does not restore the original <see cref="IText"/> tree.
		/// </summary>
		[XmlElement]
		public String Description
		{ get; set; }

		/// <summary>Surrogates of the statements the answer was derived from.</summary>
		[XmlArray(nameof(Explanation))]
		public List<Statement> Explanation
		{ get; set; }

		/// <summary>Whether the answer means "unknown".</summary>
		[XmlElement]
		public Boolean IsEmpty
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates a surrogate of the "unknown" answer, as required by the XML serializer.</summary>
		public Answer()
			: this(AabSemantics.Answers.Answer.CreateUnknown(), Language.Default)
		{ }

		/// <summary>Converts an answer into its surrogate, rendering its text in the given language.</summary>
		/// <param name="answer">Answer to convert.</param>
		/// <param name="language">Language its text is rendered in.</param>
		public Answer(IAnswer answer, ILanguage language)
		{
			Description = TextRenders.PlainString.Render(answer.Description, language).ToString();
			Explanation = answer.Explanation.Statements.Select(statement => Statement.Load(statement)).ToList();
			IsEmpty = answer.IsEmpty;
		}

		#endregion

		/// <summary>Converts an answer into the surrogate registered for its type.</summary>
		/// <param name="answer">Answer to convert.</param>
		/// <param name="language">Language its text is rendered in.</param>
		/// <returns>The surrogate, ready to be serialized.</returns>
		/// <exception cref="NotSupportedException">The answer's type is not registered.</exception>
		public static Answer Load(IAnswer answer, ILanguage language)
		{
			var definition = Repositories.Answers.Definitions.GetSuitable(answer);
			return definition.GetSerializationSettings<AnswerXmlSerializationSettings>().GetXml(answer, language);
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
			var statementOverrides = Repositories.Statements.Definitions.Values.ToDictionary(
				definition => definition.GetSerializationSettings<StatementXmlSerializationSettings>().XmlElementName,
				definition => definition.GetSerializationSettings<StatementXmlSerializationSettings>().XmlType);

			typeof(Answer).DefineTypeOverride(new XmlHelper.PropertyTypes(nameof(Explanation), typeof(Answer), statementOverrides));
			typeof(BooleanAnswer).DefineTypeOverride(new XmlHelper.PropertyTypes(nameof(Explanation), typeof(Answer), statementOverrides));
			typeof(ConceptAnswer).DefineTypeOverride(new XmlHelper.PropertyTypes(nameof(Explanation), typeof(Answer), statementOverrides));
			typeof(ConceptsAnswer).DefineTypeOverride(new XmlHelper.PropertyTypes(nameof(Explanation), typeof(Answer), statementOverrides));
			typeof(StatementAnswer).DefineTypeOverrides(new[]
			{
				new XmlHelper.PropertyTypes(nameof(Explanation), typeof(StatementAnswer), statementOverrides),
				new XmlHelper.PropertyTypes(nameof(StatementAnswer.Statement), typeof(StatementAnswer), statementOverrides),
			});
			typeof(StatementsAnswer).DefineTypeOverrides(new[]
			{
				new XmlHelper.PropertyTypes(nameof(Explanation), typeof(StatementsAnswer), statementOverrides),
				new XmlHelper.PropertyTypes(nameof(StatementsAnswer.Statements), typeof(StatementsAnswer), statementOverrides),
			});
		}
	}
}
