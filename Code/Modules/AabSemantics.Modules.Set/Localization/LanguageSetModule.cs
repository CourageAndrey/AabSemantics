using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Set.Localization
{
	public interface ILanguageSetModule : ILanguageAttributesExtension<ILanguageAttributes>, ILanguageStatementsExtension<ILanguageStatements>, ILanguageQuestionsExtension<ILanguageQuestions>
	{ }

	[XmlType]
	public class LanguageSetModule : LanguageExtension, ILanguageSetModule
	{
		#region Xml Properties

		[XmlElement(nameof(Attributes))]
		public LanguageAttributes AttributesXml
		{ get; set; }

		[XmlElement(nameof(Statements))]
		public LanguageStatements StatementsXml
		{ get; set; }

		[XmlElement(nameof(Questions))]
		public LanguageQuestions QuestionsXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		ILanguageExtensionAttributes ILanguageAttributesExtension.Attributes
		{ get { return AttributesXml; } }

		[XmlIgnore]
		ILanguageExtensionStatements ILanguageStatementsExtension.Statements
		{ get { return StatementsXml; } }

		[XmlIgnore]
		ILanguageExtensionQuestions ILanguageQuestionsExtension.Questions
		{ get { return QuestionsXml; } }

		[XmlIgnore]
		public ILanguageAttributes Attributes
		{ get { return AttributesXml; } }

		[XmlIgnore]
		public ILanguageStatements Statements
		{ get { return StatementsXml; } }

		[XmlIgnore]
		public ILanguageQuestions Questions
		{ get { return QuestionsXml; } }

		#endregion

		public static LanguageSetModule CreateDefault()
		{
			return new LanguageSetModule()
			{
				AttributesXml = LanguageAttributes.CreateDefault(),
				StatementsXml = LanguageStatements.CreateDefault(),
				QuestionsXml = LanguageQuestions.CreateDefault(),
			};
		}
	}
}
