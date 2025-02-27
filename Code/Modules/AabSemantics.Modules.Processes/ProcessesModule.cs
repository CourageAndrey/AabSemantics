using System;
using System.Collections.Generic;

using AabSemantics.Metadata;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Concepts;
using AabSemantics.Modules.Processes.Localization;
using AabSemantics.Modules.Processes.Questions;
using AabSemantics.Modules.Processes.Statements;
using AabSemantics.Serialization;

namespace AabSemantics.Modules.Processes
{
	public class ProcessesModule : ExtensionModule
	{
		public const String ModuleName = "System.Processes";

		public ProcessesModule()
			: base(ModuleName)
		{ }

		protected override void Attach(ISemanticNetwork semanticNetwork)
		{
			foreach (var sign in SequenceSigns.All)
			{
				semanticNetwork.Concepts.Add(sign);
			}
		}

		protected override void RegisterLanguage()
		{
			AabSemantics.Localization.Language.Default.Extensions.Add(LanguageProcessesModule.CreateDefault());
		}

		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(IsProcessAttribute.Value, language => language.GetAttributesExtension<ILanguageProcessesModule, ILanguageAttributes>().IsProcess)
				.SerializeToXml(new Xml.IsProcessAttribute())
				.SerializeToJson(new Xml.IsProcessAttribute());
			Repositories.RegisterAttribute(IsSequenceSignAttribute.Value, language => language.GetAttributesExtension<ILanguageProcessesModule, ILanguageAttributes>().IsSequenceSign)
				.SerializeToXml(new Xml.IsSequenceSignAttribute())
				.SerializeToJson(new Xml.IsSequenceSignAttribute());
		}

		protected override void RegisterConcepts()
		{
			ConceptIdResolver.RegisterEnumType(typeof(SequenceSigns));
		}

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<ProcessesStatement>(
					language => language.GetStatementsExtension<ILanguageProcessesModule, ILanguageStatements>().Names.Processes,
					language => language.GetStatementsExtension<ILanguageProcessesModule, ILanguageStatements>().TrueFormatStrings.Processes,
					language => language.GetStatementsExtension<ILanguageProcessesModule, ILanguageStatements>().FalseFormatStrings.Processes,
					language => language.GetStatementsExtension<ILanguageProcessesModule, ILanguageStatements>().QuestionFormatStrings.Processes,
					statement => new Dictionary<String, IKnowledge>
					{
						{ Strings.ParamProcessA, statement.ProcessA },
						{ Strings.ParamProcessB, statement.ProcessB },
						{ Strings.ParamSequenceSign, statement.SequenceSign },
					},
					checkProcessSequenceSystems)
				.SerializeToXml(statement => new Xml.ProcessesStatement(statement))
				.SerializeToJson(statement => new Json.ProcessesStatement(statement));
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<ProcessesQuestion>(language => language.GetQuestionsExtension<ILanguageProcessesModule, ILanguageQuestions>().Names.ProcessesQuestion)
				.SerializeToXml(question => new Xml.ProcessesQuestion(question))
				.SerializeToJson(question => new Json.ProcessesQuestion(question));
		}

		public override IDictionary<String, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>
			{
				{ nameof(ProcessesModule), typeof(LanguageProcessesModule) }
			};
		}

		private static void checkProcessSequenceSystems(
			ISemanticNetwork semanticNetwork,
			ITextContainer result,
			ICollection<ProcessesStatement> statements)
		{
			foreach (var contradiction in statements.CheckForContradictions())
			{
				result
					.Append(
						language => language.GetStatementsExtension<ILanguageProcessesModule, ILanguageStatements>().Consistency.ErrorProcessesContradiction,
						new Dictionary<String, IKnowledge>
						{
							{ Localization.Strings.ParamProcessA, contradiction.Value1 },
							{ Localization.Strings.ParamProcessB, contradiction.Value2 },
						})
					.Append(contradiction.Signs.EnumerateOneLine());
			}
		}
	}
}
