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

	[XmlType]
	public abstract class LanguageExtension : ILanguageExtension
	{
		[XmlIgnore]
		public abstract ILanguageExtensionAttributes Attributes
		{ get; }

		[XmlIgnore]
		public abstract ILanguageExtensionConcepts Concepts
		{ get; }

		[XmlIgnore]
		public abstract ILanguageExtensionStatements Statements
		{ get; }

		[XmlIgnore]
		public abstract ILanguageExtensionQuestions Questions
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
		: LanguageExtension, ILanguageExtension<AttributesT, ConceptsT, StatementsT, QuestionsT>
		where AttributesT : ILanguageExtensionAttributes
		where ConceptsT : ILanguageExtensionConcepts
		where StatementsT : ILanguageExtensionStatements
		where QuestionsT : ILanguageExtensionQuestions
	{
		#region Typed interface properties

		[XmlIgnore]
		AttributesT ILanguageExtension<AttributesT, ConceptsT, StatementsT, QuestionsT>.Attributes
		{ get { return AttributesXml; } }

		[XmlIgnore]
		ConceptsT ILanguageExtension<AttributesT, ConceptsT, StatementsT, QuestionsT>.Concepts
		{ get { return ConceptsXml; } }

		[XmlIgnore]
		StatementsT ILanguageExtension<AttributesT, ConceptsT, StatementsT, QuestionsT>.Statements
		{ get { return StatementsXml; } }

		[XmlIgnore]
		QuestionsT ILanguageExtension<AttributesT, ConceptsT, StatementsT, QuestionsT>.Questions
		{ get { return QuestionsXml; } }

		#endregion

		#region Untyped interface properties

		[XmlIgnore]
		public override ILanguageExtensionAttributes Attributes
		{ get { return AttributesXml; } }

		[XmlIgnore]
		public override ILanguageExtensionConcepts Concepts
		{ get { return ConceptsXml; } }

		[XmlIgnore]
		public override ILanguageExtensionStatements Statements
		{ get { return StatementsXml; } }

		[XmlIgnore]
		public override ILanguageExtensionQuestions Questions
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