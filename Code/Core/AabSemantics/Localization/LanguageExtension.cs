using System.Xml.Serialization;

namespace AabSemantics.Localization
{
	public interface ILanguageExtension
	{ }

	public interface ILanguageExtensionAttributes
	{ }

	public interface ILanguageExtensionConcepts
	{ }

	public interface ILanguageExtensionStatements
	{ }

	public interface ILanguageExtensionQuestions
	{ }

	public interface ILanguageAttributesExtension : ILanguageExtension
	{
		ILanguageExtensionAttributes Attributes
		{ get; }
	}

	public interface ILanguageConceptsExtension : ILanguageExtension
	{
		ILanguageExtensionConcepts Concepts
		{ get; }
	}

	public interface ILanguageStatementsExtension : ILanguageExtension
	{
		ILanguageExtensionStatements Statements
		{ get; }
	}

	public interface ILanguageQuestionsExtension : ILanguageExtension
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
	public class LanguageExtension : ILanguageExtension
	{ }
}