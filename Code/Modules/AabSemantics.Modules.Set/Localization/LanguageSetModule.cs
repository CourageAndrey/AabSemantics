using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Set.Localization
{
	/// <summary>The set module's string bundle: its attribute, statement and question texts.</summary>
	public interface ILanguageSetModule : ILanguageAttributesExtension<ILanguageAttributes>, ILanguageStatementsExtension<ILanguageStatements>, ILanguageQuestionsExtension<ILanguageQuestions>
	{ }

	/// <summary>Serializable <see cref="ILanguageSetModule"/>, loaded from a language file.</summary>
	[XmlType]
	public class LanguageSetModule : LanguageExtension, ILanguageSetModule
	{
		#region Xml Properties

		/// <summary>Attribute names contributed by the module. In serializable form.</summary>
		[XmlElement(nameof(Attributes))]
		public LanguageAttributes AttributesXml
		{ get; set; }

		/// <summary>Statement wordings contributed by the module. In serializable form.</summary>
		[XmlElement(nameof(Statements))]
		public LanguageStatements StatementsXml
		{ get; set; }

		/// <summary>Question wordings contributed by the module. In serializable form.</summary>
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

		/// <summary>Attribute names contributed by the module.</summary>
		[XmlIgnore]
		public ILanguageAttributes Attributes
		{ get { return AttributesXml; } }

		/// <summary>Statement wordings contributed by the module.</summary>
		[XmlIgnore]
		public ILanguageStatements Statements
		{ get { return StatementsXml; } }

		/// <summary>Question wordings contributed by the module.</summary>
		[XmlIgnore]
		public ILanguageQuestions Questions
		{ get { return QuestionsXml; } }

		#endregion

		/// <summary>Builds the bundle with its built-in English texts.</summary>
		/// <returns>A fully populated bundle.</returns>
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
