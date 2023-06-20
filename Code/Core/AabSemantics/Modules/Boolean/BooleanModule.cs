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
	public class BooleanModule : ExtensionModule
	{
		public const String ModuleName = "System.Boolean";

		public BooleanModule()
			: base(ModuleName)
		{ }

		protected override void Attach(ISemanticNetwork semanticNetwork)
		{
			foreach (var boolean in LogicalValues.All)
			{
				semanticNetwork.Concepts.Add(boolean);
			}
		}

		protected override void RegisterLanguage()
		{
			AabSemantics.Localization.Language.Default.Extensions.Add(LanguageBooleanModule.CreateDefault());
		}

		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(IsValueAttribute.Value, language => language.GetExtension<ILanguageBooleanModule>().Attributes.IsValue)
				.SerializeToXml(new Xml.IsValueAttribute())
				.SerializeToJson(new Xml.IsValueAttribute());
			Repositories.RegisterAttribute(IsBooleanAttribute.Value, language => language.GetExtension<ILanguageBooleanModule>().Attributes.IsBoolean)
				.SerializeToXml(new Xml.IsBooleanAttribute())
				.SerializeToJson(new Xml.IsBooleanAttribute());
		}

		protected override void RegisterConcepts()
		{
			ConceptIdResolver.RegisterEnumType(typeof(LogicalValues));
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<CheckStatementQuestion>(language => language.GetExtension<ILanguageBooleanModule>().Questions.Names.CheckStatementQuestion)
				.SerializeToXml(question => new Xml.CheckStatementQuestion(question))
				.SerializeToJson(question => new Json.CheckStatementQuestion(question));
		}

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

		public override IDictionary<String, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>
			{
				{ nameof(BooleanModule), typeof(LanguageBooleanModule) }
			};
		}
	}
}
