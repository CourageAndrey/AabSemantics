using System.Xml.Serialization;

namespace AabSemantics.Localization
{
	public interface ILanguageExtension
	{
		ILanguageExtensionAttributes Attributes
		{ get; }

		ILanguageExtensionConcepts Concepts
		{ get; }

		ILanguageExtensionStatements Statements
		{ get; }

		ILanguageExtensionQuestions Questions
		{ get; }
	}

	public interface ILanguageExtension<out AttributesT, out ConceptsT, out StatementsT, out QuestionsT> : ILanguageExtension
		where AttributesT : ILanguageExtensionAttributes
		where ConceptsT : ILanguageExtensionConcepts
		where StatementsT : ILanguageExtensionStatements
		where QuestionsT : ILanguageExtensionQuestions
	{
		new AttributesT Attributes
		{ get; }

		new ConceptsT Concepts
		{ get; }

		new StatementsT Statements
		{ get; }

		new QuestionsT Questions
		{ get; }
	}

	[XmlType]
	public class LanguageExtension<AttributesT, ConceptsT, StatementsT, QuestionsT>
		: ILanguageExtension<AttributesT, ConceptsT, StatementsT, QuestionsT>
		where AttributesT : ILanguageExtensionAttributes
		where ConceptsT : ILanguageExtensionConcepts
		where StatementsT : ILanguageExtensionStatements
		where QuestionsT : ILanguageExtensionQuestions
	{
		#region Typed interface properties

		[XmlElement]
		public AttributesT Attributes
		{ get { return AttributesXml; } }

		[XmlElement]
		public ConceptsT Concepts
		{ get { return ConceptsXml; } }

		[XmlElement]
		public StatementsT Statements
		{ get { return StatementsXml; } }

		[XmlElement]
		public QuestionsT Questions
		{ get { return QuestionsXml; } }

		#endregion

		#region Untyped interface properties

		ILanguageExtensionAttributes ILanguageExtension.Attributes
		{ get { return AttributesXml; } }

		ILanguageExtensionConcepts ILanguageExtension.Concepts
		{ get { return ConceptsXml; } }

		ILanguageExtensionStatements ILanguageExtension.Statements
		{ get { return StatementsXml; } }

		ILanguageExtensionQuestions ILanguageExtension.Questions
		{ get { return QuestionsXml; } }

		#endregion

		#region Xml Properties

		[XmlElement(nameof(Attributes))]
		public AttributesT AttributesXml
		{ get; set; }

		[XmlElement(nameof(Concepts))]
		public ConceptsT ConceptsXml
		{ get; set; }

		[XmlElement(nameof(Statements))]
		public StatementsT StatementsXml
		{ get; set; }

		[XmlElement(nameof(Questions))]
		public QuestionsT QuestionsXml
		{ get; set; }

		#endregion
	}
}