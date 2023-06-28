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
			: this(AabSemantics.Answers.Answer.CreateUnknown(), Language.Default)
		{ }

		public Answer(IAnswer answer, ILanguage language)
		{
			Description = TextRenders.PlainString.Render(answer.Description, language).ToString();
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
			return new AabSemantics.Answers.Answer(
				new FormattedText(language => Description, new Dictionary<String, IKnowledge>()),
				new Explanation(Explanation.Select(statement => statement.SaveOrReuse(conceptIdResolver, statementIdResolver))),
				IsEmpty);
		}

		static Answer()
		{
			var statementOverrides = Repositories.Statements.Definitions.Values.ToDictionary(
				definition => definition.GetXmlSerializationSettings<StatementXmlSerializationSettings>().XmlElementName,
				definition => definition.GetXmlSerializationSettings<StatementXmlSerializationSettings>().XmlType);

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
