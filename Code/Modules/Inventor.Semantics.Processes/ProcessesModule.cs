using System;
using System.Collections.Generic;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Metadata;
using Inventor.Semantics.Processes.Attributes;
using Inventor.Semantics.Processes.Concepts;
using Inventor.Semantics.Processes.Localization;
using Inventor.Semantics.Processes.Questions;
using Inventor.Semantics.Processes.Statements;
using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Processes
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
			Language.Default.Extensions.Add(LanguageProcessesModule.CreateDefault());
		}

		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(IsProcessAttribute.Value, language => language.GetExtension<ILanguageProcessesModule>().Attributes.IsProcess, new Xml.IsProcessAttribute());
			Repositories.RegisterAttribute(IsSequenceSignAttribute.Value, language => language.GetExtension<ILanguageProcessesModule>().Attributes.IsSequenceSign, new Xml.IsSequenceSignAttribute());
		}

		protected override void RegisterConcepts()
		{
			ConceptIdResolver.RegisterEnumType(typeof(SequenceSigns));
		}

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<ProcessesStatement, Xml.ProcessesStatement>(
				language => language.GetExtension<ILanguageProcessesModule>().Statements.Names.Processes,
				statement => new Xml.ProcessesStatement(statement),
				checkProcessSequenceSystems);
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<ProcessesQuestion, Xml.ProcessesQuestion>(
				language => language.GetExtension<ILanguageProcessesModule>().Questions.Names.ProcessesQuestion,
				question => new Xml.ProcessesQuestion(question));
		}

		public override IDictionary<String, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>
			{
				{ nameof(ProcessesModule), typeof(LanguageProcessesModule) }
			};
		}

		private static void checkProcessSequenceSystems(
			ICollection<ProcessesStatement> statements,
			ITextContainer result,
			ISemanticNetwork semanticNetwork)
		{
			foreach (var contradiction in statements.CheckForContradictions())
			{
				result
					.Append(
						language => language.GetExtension<ILanguageProcessesModule>().Statements.Consistency.ErrorProcessesContradiction,
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
