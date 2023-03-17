using System;
using System.Collections.Generic;

using Inventor.Semantics.Metadata;
using Inventor.Semantics.Modules.Boolean.Attributes;
using Inventor.Semantics.Modules.Boolean.Concepts;
using Inventor.Semantics.Modules.Boolean.Localization;
using Inventor.Semantics.Modules.Boolean.Questions;
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
			Semantics.Xml.ConceptIdResolver.RegisterEnumType(typeof(LogicalValues));
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<CheckStatementQuestion>(
				language => language.GetExtension<ILanguageBooleanModule>().Questions.Names.CheckStatementQuestion,
				question => new Xml.CheckStatementQuestion(question),
				typeof(Xml.CheckStatementQuestion));
		}

		protected override void RegisterAnswers()
		{
			Repositories.RegisterAnswer<Answers.Answer>((answer, language) => new Answer(answer, language), typeof(Answer));
			Repositories.RegisterAnswer<Answers.BooleanAnswer>((answer, language) => new BooleanAnswer(answer, language), typeof(BooleanAnswer));
			Repositories.RegisterAnswer<Answers.ConceptAnswer>((answer, language) => new ConceptAnswer(answer, language), typeof(ConceptAnswer));
			Repositories.RegisterAnswer<Answers.ConceptsAnswer>((answer, language) => new ConceptsAnswer(answer, language), typeof(ConceptsAnswer));
			Repositories.RegisterAnswer<Answers.StatementAnswer>((answer, language) => new StatementAnswer(answer, language), typeof(StatementAnswer));
			Repositories.RegisterAnswer<Answers.StatementsAnswer>((answer, language) => new StatementsAnswer(answer, language), typeof(StatementsAnswer));
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
