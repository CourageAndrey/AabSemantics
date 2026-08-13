using System.Xml.Serialization;

namespace AabSemantics.Localization
{
	/// <summary>
	/// Marker for a module's string bundle. Each module contributes one, and the untyped/typed
	/// interface pairs below let the engine read a bundle generically while module code reads it
	/// in its own concrete type.
	/// </summary>
	public interface ILanguageExtension
	{ }

	/// <summary>Marker for a module's attribute names.</summary>
	public interface ILanguageExtensionAttributes
	{ }

	/// <summary>Marker for a module's concept names.</summary>
	public interface ILanguageExtensionConcepts
	{ }

	/// <summary>Marker for a module's statement wordings.</summary>
	public interface ILanguageExtensionStatements
	{ }

	/// <summary>
	/// A module's statement wordings, grouped so that one group holds the same field per statement
	/// type. A statement definition then picks its own wording out of each group with a single selector.
	/// </summary>
	/// <typeparam name="PartT">Type holding one field per statement type of the module.</typeparam>
	public interface ILanguageExtensionStatements<out PartT> : ILanguageExtensionStatements
	{
		/// <summary>Display names.</summary>
		PartT Names
		{ get; }

		/// <summary>Tooltip texts.</summary>
		PartT Hints
		{ get; }

		/// <summary>Affirmative wordings.</summary>
		PartT TrueFormatStrings
		{ get; }

		/// <summary>Negative wordings.</summary>
		PartT FalseFormatStrings
		{ get; }

		/// <summary>Interrogative wordings.</summary>
		PartT QuestionFormatStrings
		{ get; }
	}

	/// <summary>Marker for a module's question wordings.</summary>
	public interface ILanguageExtensionQuestions
	{ }

	/// <summary>A module bundle that contributes attribute names.</summary>
	public interface ILanguageAttributesExtension : ILanguageExtension
	{
		/// <summary>The module's attribute names.</summary>
		ILanguageExtensionAttributes Attributes
		{ get; }
	}

	/// <summary>A module bundle that contributes concept names.</summary>
	public interface ILanguageConceptsExtension : ILanguageExtension
	{
		/// <summary>The module's concept names.</summary>
		ILanguageExtensionConcepts Concepts
		{ get; }
	}

	/// <summary>A module bundle that contributes statement wordings.</summary>
	public interface ILanguageStatementsExtension : ILanguageExtension
	{
		/// <summary>The module's statement wordings.</summary>
		ILanguageExtensionStatements Statements
		{ get; }
	}

	/// <summary>A module bundle that contributes question wordings.</summary>
	public interface ILanguageQuestionsExtension : ILanguageExtension
	{
		/// <summary>The module's question wordings.</summary>
		ILanguageExtensionQuestions Questions
		{ get; }
	}

	/// <summary>Attribute-name bundle exposed in the module's own type.</summary>
	/// <typeparam name="AttributesT">Concrete attribute-names type.</typeparam>
	public interface ILanguageAttributesExtension<out AttributesT> : ILanguageAttributesExtension
		where AttributesT : ILanguageExtensionAttributes
	{
		/// <summary>The module's attribute names, strongly typed.</summary>
		new AttributesT Attributes
		{ get; }
	}

	/// <summary>Concept-name bundle exposed in the module's own type.</summary>
	/// <typeparam name="ConceptsT">Concrete concept-names type.</typeparam>
	public interface ILanguageConceptsExtension<out ConceptsT> : ILanguageConceptsExtension
		where ConceptsT : ILanguageExtensionConcepts
	{
		/// <summary>The module's concept names, strongly typed.</summary>
		new ConceptsT Concepts
		{ get; }
	}

	/// <summary>Statement-wording bundle exposed in the module's own type.</summary>
	/// <typeparam name="StatementsT">Concrete statement-wordings type.</typeparam>
	public interface ILanguageStatementsExtension<out StatementsT> : ILanguageStatementsExtension
		where StatementsT : ILanguageExtensionStatements
	{
		/// <summary>The module's statement wordings, strongly typed.</summary>
		new StatementsT Statements
		{ get; }
	}

	/// <summary>Question-wording bundle exposed in the module's own type.</summary>
	/// <typeparam name="QuestionsT">Concrete question-wordings type.</typeparam>
	public interface ILanguageQuestionsExtension<out QuestionsT> : ILanguageQuestionsExtension
		where QuestionsT : ILanguageExtensionQuestions
	{
		/// <summary>The module's question wordings, strongly typed.</summary>
		new QuestionsT Questions
		{ get; }
	}

	/// <summary>
	/// Base class for a module's string bundle. Modules derive from it and add their own
	/// serializable properties.
	/// </summary>
	[XmlType]
	public class LanguageExtension : ILanguageExtension
	{ }
}