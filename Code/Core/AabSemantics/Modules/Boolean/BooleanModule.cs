using System;
using System.Collections.Generic;

using AabSemantics.Metadata;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Boolean.Concepts;
using AabSemantics.Modules.Boolean.Localization;
using AabSemantics.Modules.Boolean.Questions;
using AabSemantics.Serialization;

namespace AabSemantics.Modules.Boolean
{
	/// <summary>
	/// Built-in module supplying the logical values, the value attributes, the
	/// "is this statement true" question, and the serialization of every core answer type.
	/// It has no dependencies, so it is normally the first module attached.
	/// </summary>
	public class BooleanModule : ExtensionModule
	{
		/// <summary>Name the module is registered under.</summary>
		public const String ModuleName = "System.Boolean";

		/// <summary>Creates the module.</summary>
		public BooleanModule()
			: base(ModuleName)
		{ }

		/// <summary>Adds the logical value concepts to the network.</summary>
		/// <param name="semanticNetwork">Network being extended.</param>
		protected override void Attach(ISemanticNetwork semanticNetwork)
		{
			foreach (var boolean in LogicalValues.All)
			{
				semanticNetwork.Concepts.Add(boolean);
			}
		}

		/// <summary>Adds the module's English texts to the built-in default language.</summary>
		protected override void RegisterLanguage()
		{
			AabSemantics.Localization.Language.Default.Extensions.Add(LanguageBooleanModule.CreateDefault());
		}

		/// <summary>Registers the "is a value" and "is a logical value" attributes.</summary>
		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(IsValueAttribute.Value, language => language.GetAttributesExtension<ILanguageBooleanModule, ILanguageAttributes>().IsValue)
				.SerializeToXml(new Xml.IsValueAttribute())
				.SerializeToJson(new Xml.IsValueAttribute());
			Repositories.RegisterAttribute(IsBooleanAttribute.Value, language => language.GetAttributesExtension<ILanguageBooleanModule, ILanguageAttributes>().IsBoolean)
				.SerializeToXml(new Xml.IsBooleanAttribute())
				.SerializeToJson(new Xml.IsBooleanAttribute());
		}

		/// <summary>Makes the logical value concepts resolvable by identifier during deserialization.</summary>
		protected override void RegisterConcepts()
		{
			ConceptIdResolver.RegisterEnumType(typeof(LogicalValues));
		}

		/// <summary>Registers the "is this statement true" question and its persistence.</summary>
		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<CheckStatementQuestion>(language => language.GetQuestionsExtension<ILanguageBooleanModule, ILanguageQuestions>().Names.CheckStatementQuestion)
				.SerializeToXml(question => new Xml.CheckStatementQuestion(question))
				.SerializeToJson(question => new Json.CheckStatementQuestion(question));
		}

		/// <summary>
		/// Registers persistence for every core answer type. These belong to the engine rather
		/// than to this module, but some module has to register them, and this one is always present.
		/// </summary>
		protected override void RegisterAnswers()
		{
			Repositories.RegisterAnswer<Answers.Answer>()
				.SerializeToXml((answer, language) => new Serialization.Xml.Answer(answer, language))
				.SerializeToJson((answer, language) => new Serialization.Json.Answer(answer, language));
			Repositories.RegisterAnswer<Answers.BooleanAnswer>()
				.SerializeToXml((answer, language) => new Serialization.Xml.Answers.BooleanAnswer(answer, language))
				.SerializeToJson((answer, language) => new Serialization.Json.Answers.BooleanAnswer(answer, language));
			Repositories.RegisterAnswer<Answers.ConceptAnswer>()
				.SerializeToXml((answer, language) => new Serialization.Xml.Answers.ConceptAnswer(answer, language))
				.SerializeToJson((answer, language) => new Serialization.Json.Answers.ConceptAnswer(answer, language));
			Repositories.RegisterAnswer<Answers.ConceptsAnswer>()
				.SerializeToXml((answer, language) => new Serialization.Xml.Answers.ConceptsAnswer(answer, language))
				.SerializeToJson((answer, language) => new Serialization.Json.Answers.ConceptsAnswer(answer, language));
			Repositories.RegisterAnswer<Answers.StatementAnswer>()
				.SerializeToXml((answer, language) => new Serialization.Xml.Answers.StatementAnswer(answer, language))
				.SerializeToJson((answer, language) => new Serialization.Json.Answers.StatementAnswer(answer, language));
			Repositories.RegisterAnswer<Answers.StatementsAnswer>()
				.SerializeToXml((answer, language) => new Serialization.Xml.Answers.StatementsAnswer(answer, language))
				.SerializeToJson((answer, language) => new Serialization.Json.Answers.StatementsAnswer(answer, language));
		}

		/// <summary>Declares the module's string bundle type for the XML serializer.</summary>
		/// <returns>A single entry mapping the module name to its bundle type.</returns>
		public override IDictionary<String, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>
			{
				{ nameof(BooleanModule), typeof(LanguageBooleanModule) }
			};
		}
	}
}
