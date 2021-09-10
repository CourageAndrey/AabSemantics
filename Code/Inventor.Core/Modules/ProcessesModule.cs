using System;
using System.Collections.Generic;

using Inventor.Core.Attributes;
using Inventor.Core.Concepts;
using Inventor.Core.Localization;
using Inventor.Core.Localization.Modules;
using Inventor.Core.Metadata;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Modules
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

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<ProcessesStatement>(
				language => language.GetExtension<ILanguageProcessesModule>().Statements.Names.Processes,
				statement => new Xml.ProcessesStatement(statement),
				typeof(Xml.ProcessesStatement),
				checkProcessSequenceSystems);
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<ProcessesQuestion>(language => language.GetExtension<ILanguageProcessesModule>().Questions.Names.ProcessesQuestion);
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
							{ Strings.ParamProcessA, contradiction.Value1 },
							{ Strings.ParamProcessB, contradiction.Value2 },
						})
					.Append(contradiction.Signs.EnumerateOneLine());
			}
		}
	}
}
