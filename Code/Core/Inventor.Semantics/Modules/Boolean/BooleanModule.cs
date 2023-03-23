using System;
using System.Collections.Generic;

using Inventor.Semantics.Metadata;
using Inventor.Semantics.Modules.Boolean.Attributes;
using Inventor.Semantics.Modules.Boolean.Concepts;
using Inventor.Semantics.Modules.Boolean.Localization;
using Inventor.Semantics.Modules.Boolean.Questions;
using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Modules.Boolean
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
			Semantics.Localization.Language.Default.Extensions.Add(LanguageBooleanModule.CreateDefault());
		}

		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(IsValueAttribute.Value, language => language.GetExtension<ILanguageBooleanModule>().Attributes.IsValue, new Xml.IsValueAttribute());
			Repositories.RegisterAttribute(IsBooleanAttribute.Value, language => language.GetExtension<ILanguageBooleanModule>().Attributes.IsBoolean, new Xml.IsBooleanAttribute());
		}

		protected override void RegisterConcepts()
		{
			ConceptIdResolver.RegisterEnumType(typeof(LogicalValues));
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<CheckStatementQuestion, Xml.CheckStatementQuestion>(
				language => language.GetExtension<ILanguageBooleanModule>().Questions.Names.CheckStatementQuestion,
				question => new Xml.CheckStatementQuestion(question));
		}

		protected override void RegisterAnswers()
		{
			Repositories.RegisterAnswer<Answers.Answer, Serialization.Xml.Answer>(
				(answer, language) => new Serialization.Xml.Answer(answer, language));
			Repositories.RegisterAnswer<Answers.BooleanAnswer, Serialization.Xml.Answers.BooleanAnswer>(
				(answer, language) => new Serialization.Xml.Answers.BooleanAnswer(answer, language));
			Repositories.RegisterAnswer<Answers.ConceptAnswer, Serialization.Xml.Answers.ConceptAnswer>(
				(answer, language) => new Serialization.Xml.Answers.ConceptAnswer(answer, language));
			Repositories.RegisterAnswer<Answers.ConceptsAnswer, Serialization.Xml.Answers.ConceptsAnswer>(
				(answer, language) => new Serialization.Xml.Answers.ConceptsAnswer(answer, language));
			Repositories.RegisterAnswer<Answers.StatementAnswer, Serialization.Xml.Answers.StatementAnswer>(
				(answer, language) => new Serialization.Xml.Answers.StatementAnswer(answer, language));
			Repositories.RegisterAnswer<Answers.StatementsAnswer, Serialization.Xml.Answers.StatementsAnswer>(
				(answer, language) => new Serialization.Xml.Answers.StatementsAnswer(answer, language));
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
