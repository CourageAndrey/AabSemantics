using System;
using System.Collections.Generic;

using Inventor.Semantics.Metadata;
using Inventor.Semantics.Modules.Boolean.Attributes;
using Inventor.Semantics.Modules.Boolean.Concepts;
using Inventor.Semantics.Modules.Boolean.Localization;
using Inventor.Semantics.Modules.Boolean.Questions;
using Inventor.Semantics.Serialization;
using Inventor.Semantics.Xml;
using Inventor.Semantics.Xml.Answers;

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
			Repositories.RegisterAnswer<Answers.Answer, Answer>((answer, language) => new Answer(answer, language));
			Repositories.RegisterAnswer<Answers.BooleanAnswer, BooleanAnswer>((answer, language) => new BooleanAnswer(answer, language));
			Repositories.RegisterAnswer<Answers.ConceptAnswer, ConceptAnswer>((answer, language) => new ConceptAnswer(answer, language));
			Repositories.RegisterAnswer<Answers.ConceptsAnswer, ConceptsAnswer>((answer, language) => new ConceptsAnswer(answer, language));
			Repositories.RegisterAnswer<Answers.StatementAnswer, StatementAnswer>((answer, language) => new StatementAnswer(answer, language));
			Repositories.RegisterAnswer<Answers.StatementsAnswer, StatementsAnswer>((answer, language) => new StatementsAnswer(answer, language));
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
