using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Metadata;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Concepts;
using AabSemantics.Modules.Processes.Localization;
using AabSemantics.Modules.Processes.Questions;
using AabSemantics.Modules.Processes.Statements;
using AabSemantics.Serialization;

namespace AabSemantics.Modules.Processes
{
	/// <summary>
	/// Built-in module supplying the fifteen sequence signs, the process sequence statement with
	/// its contradiction check, and the sequence question. Depends on the boolean module.
	/// </summary>
	public class ProcessesModule : ExtensionModule
	{
		/// <summary>Name the module is registered under.</summary>
		public const String ModuleName = "System.Processes";

		/// <summary>Creates the module, declaring its dependency on the boolean module.</summary>
		public ProcessesModule()
			: base(ModuleName)
		{ }

		/// <summary>Adds the sequence sign concepts to the network.</summary>
		/// <param name="semanticNetwork">Network being extended.</param>
		protected override void Attach(ISemanticNetwork semanticNetwork)
		{
			foreach (var sign in SequenceSigns.All)
			{
				semanticNetwork.Concepts.Add(sign);
			}
		}

		/// <summary>Adds the module's English texts to the built-in default language.</summary>
		protected override void RegisterLanguage()
		{
			AabSemantics.Localization.Language.Default.Extensions.Add(LanguageProcessesModule.CreateDefault());
		}

		/// <summary>Registers the "is a process" and "is a sequence sign" attributes.</summary>
		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(IsProcessAttribute.Value, language => language.GetAttributesExtension<ILanguageProcessesModule, ILanguageAttributes>().IsProcess)
				.SerializeToXml(new Xml.IsProcessAttribute())
				.SerializeToJson(new Xml.IsProcessAttribute());
			Repositories.RegisterAttribute(IsSequenceSignAttribute.Value, language => language.GetAttributesExtension<ILanguageProcessesModule, ILanguageAttributes>().IsSequenceSign)
				.SerializeToXml(new Xml.IsSequenceSignAttribute())
				.SerializeToJson(new Xml.IsSequenceSignAttribute());
		}

		/// <summary>Makes the sequence sign concepts resolvable by identifier during deserialization.</summary>
		protected override void RegisterConcepts()
		{
			ConceptIdResolver.RegisterEnumType(typeof(SequenceSigns));
		}

		/// <summary>Registers the process sequence statement, its contradiction check and its custom-statement form.</summary>
		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<ProcessesStatement, ILanguageProcessesModule, ILanguageStatements, ILanguageStatementsPart>(
					language => language.Processes,
					statement => new Dictionary<String, IKnowledge>
					{
						{ Strings.ParamProcessA, statement.ProcessA },
						{ Strings.ParamProcessB, statement.ProcessB },
						{ Strings.ParamSequenceSign, statement.SequenceSign },
					},
					CheckProcessSequenceSystemsAsync)
				.SerializeToXml(statement => new Xml.ProcessesStatement(statement))
				.SerializeToJson(statement => new Json.ProcessesStatement(statement));
			Repositories.RegisterCustomStatement<ProcessesStatement, ILanguageProcessesModule, ILanguageStatements, ILanguageStatementsPart>(
				new List<String> { nameof(ProcessesStatement.ProcessA), nameof(ProcessesStatement.ProcessB), nameof(ProcessesStatement.SequenceSign) },
				language => language.Processes);
		}

		/// <summary>Registers the process sequence question and its persistence.</summary>
		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<ProcessesQuestion>(language => language.GetQuestionsExtension<ILanguageProcessesModule, ILanguageQuestions>().Names.ProcessesQuestion)
				.SerializeToXml(question => new Xml.ProcessesQuestion(question))
				.SerializeToJson(question => new Json.ProcessesQuestion(question));
		}

		/// <summary>Declares the module's string bundle type for the XML serializer.</summary>
		/// <returns>A single entry mapping the module name to its bundle type.</returns>
		public override IDictionary<String, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>
			{
				{ nameof(ProcessesModule), typeof(LanguageProcessesModule) }
			};
		}

		private static async Task CheckProcessSequenceSystemsAsync(
			ISemanticNetwork semanticNetwork,
			ITextContainer result,
			ICollection<ProcessesStatement> statements)
		{
			foreach (var contradiction in await statements.CheckForContradictionsAsync())
			{
				result
					.Append(
						language => language.GetStatementsExtension<ILanguageProcessesModule, ILanguageStatements>().Consistency.ErrorProcessesContradiction,
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
