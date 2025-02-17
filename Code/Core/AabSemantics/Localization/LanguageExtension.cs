using System.Xml.Serialization;

namespace AabSemantics.Localization
{
	public interface ILanguageAttributesExtension
	{
		ILanguageExtensionAttributes Attributes
		{ get; }
	}

	public interface ILanguageConceptsExtension
	{
		ILanguageExtensionConcepts Concepts
		{ get; }
	}

	public interface ILanguageStatementsExtension
	{
		ILanguageExtensionStatements Statements
		{ get; }
	}

	public interface ILanguageQuestionsExtension
	{
		ILanguageExtensionQuestions Questions
		{ get; }
	}

	public interface ILanguageAttributesExtension<out AttributesT> : ILanguageAttributesExtension
		where AttributesT : ILanguageExtensionAttributes
	{
		new AttributesT Attributes
		{ get; }
	}

	public interface ILanguageConceptsExtension<out ConceptsT> : ILanguageConceptsExtension
		where ConceptsT : ILanguageExtensionConcepts
	{
		new ConceptsT Concepts
		{ get; }
	}

	public interface ILanguageStatementsExtension<out StatementsT> : ILanguageStatementsExtension
		where StatementsT : ILanguageExtensionStatements
	{
		new StatementsT Statements
		{ get; }
	}

	public interface ILanguageQuestionsExtension<out QuestionsT> : ILanguageQuestionsExtension
		where QuestionsT : ILanguageExtensionQuestions
	{
		new QuestionsT Questions
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