using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Metadata;
using Inventor.Semantics.Serialization.Xml.Answers;
using Inventor.Semantics.Text.Primitives;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Serialization.Xml
{
	[XmlType]
	[XmlInclude(typeof(BooleanAnswer))]
	[XmlInclude(typeof(ConceptAnswer))]
	[XmlInclude(typeof(ConceptsAnswer))]
	[XmlInclude(typeof(StatementAnswer))]
	[XmlInclude(typeof(StatementsAnswer))]
	public class Answer
	{
		#region Properties

		[XmlElement]
		public String Description
		{ get; set; }

		[XmlArray(nameof(Explanation))]
		public List<Statement> Explanation
		{ get; set; }

		[XmlElement]
		public Boolean IsEmpty
		{ get; set; }

		#endregion

		#region Constructors

		public Answer()
			: this(Semantics.Answers.Answer.CreateUnknown(), Language.Default)
		{ }

		public Answer(IAnswer answer, ILanguage language)
		{
			Description = TextRepresenters.PlainString.Represent(answer.Description, language).ToString();
			Explanation = answer.Explanation.Statements.Select(statement => Statement.Load(statement)).ToList();
			IsEmpty = answer.IsEmpty;
		}

		#endregion

		public static Answer Load(IAnswer answer, ILanguage language)
		{
			var definition = Repositories.Answers.Definitions.GetSuitable(answer);
			return definition.GetXmlSerializationSettings<AnswerXmlSerializationSettings>().GetXml(answer, language);
		}

		public virtual IAnswer Save(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			return new Semantics.Answers.Answer(
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver))),
				IsEmpty);
		}

		static Answer()
		{
			var allAnswerTypes = new[]
			{
				typeof(Answer),
				typeof(BooleanAnswer),
				typeof(ConceptAnswer),
				typeof(ConceptsAnswer),
				typeof(StatementAnswer),
				typeof(StatementsAnswer),
			};

			var statementAnswerType = typeof(StatementAnswer);
			var statementsAnswerType = typeof(StatementsAnswer);
			var answerOverrides = new XmlAttributeOverrides();
			var statementAnswerOverrides = new XmlAttributeOverrides();
			var statementsAnswerOverrides = new XmlAttributeOverrides();

			var statementAttributes = new XmlAttributes();
			foreach (var definition in Repositories.Statements.Definitions.Values)
			{
				var xmlSettings = definition.GetXmlSerializationSettings<StatementXmlSerializationSettings>();
				statementAttributes.XmlElements.Add(new XmlElementAttribute(xmlSettings.XmlElementName, xmlSettings.XmlType));
			}

			XmlSerializer serializer;
			foreach (var answerType in allAnswerTypes)
			{
				answerOverrides.Add(answerType, "Explanation", statementAttributes);
				serializer = new XmlSerializer(answerType, answerOverrides);
				answerType.DefineCustomXmlSerializer(serializer);
			}

			statementAnswerOverrides.Add(statementAnswerType, "Statement", statementAttributes);
			statementsAnswerOverrides.Add(statementsAnswerType, "Statements", statementAttributes);

			serializer = new XmlSerializer(statementAnswerType, statementAnswerOverrides);
			statementAnswerType.DefineCustomXmlSerializer(serializer);

			serializer = new XmlSerializer(statementsAnswerType, statementsAnswerOverrides);
			statementsAnswerType.DefineCustomXmlSerializer(serializer);
		}
	}
}
