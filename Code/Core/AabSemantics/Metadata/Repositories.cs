using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Localization;
using AabSemantics.Utils;

namespace AabSemantics.Metadata
{
	public static class Repositories
	{
		public static IDictionary<String, IExtensionModule> Modules
		{
			get { return _modules; }
			set { _modules = value.EnsureNotNull(nameof(value)); }
		}

		public static IRepository<AttributeDefinition> Attributes
		{
			get { return _attributes; }
			set { _attributes = value.EnsureNotNull(nameof(value)); }
		}

		public static IRepository<StatementDefinition> Statements
		{
			get { return _statements; }
			set { _statements = value.EnsureNotNull(nameof(value)); }
		}

		public static IDictionary<String, CustomStatementDefinition> CustomStatements
		{
			get { return _customStatements; }
			set { _customStatements = value.EnsureNotNull(nameof(value)); }
		}

		public static IRepository<QuestionDefinition> Questions
		{
			get { return _questions; }
			set { _questions = value.EnsureNotNull(nameof(value)); }
		}

		public static IRepository<AnswerDefinition> Answers
		{
			get { return _answers; }
			set { _answers = value.EnsureNotNull(nameof(value)); }
		}

		private static IDictionary<String, IExtensionModule> _modules;
		private static IRepository<AttributeDefinition> _attributes;
		private static IRepository<StatementDefinition> _statements;
		private static IDictionary<String, CustomStatementDefinition> _customStatements;
		private static IRepository<QuestionDefinition> _questions;
		private static IRepository<AnswerDefinition> _answers;

		public static AttributeDefinition RegisterAttribute(
			Type type,
			IAttribute value,
			Func<ILanguage, String> nameGetter)
		{
			var definition = new AttributeDefinition(type, value, nameGetter);
			Attributes.Definitions[definition.Type] = definition;
			return definition;
		}

		public static AttributeDefinition<AttributeT> RegisterAttribute<AttributeT>(
			AttributeT value,
			Func<ILanguage, String> nameGetter)
			where AttributeT : IAttribute
		{
			var definition = new AttributeDefinition<AttributeT>(value, nameGetter);
			Attributes.Definitions[definition.Type] = definition;
			return definition;
		}

		public static StatementDefinition RegisterStatement(
			Type type,
			Func<ILanguage, String> nameGetter,
			Func<ILanguage, String> formatTrue,
			Func<ILanguage, String> formatFalse,
			Func<ILanguage, String> formatQuestion,
			Func<IStatement, IDictionary<String, IKnowledge>> getDescriptionParameters,
			StatementConsistencyCheckerDelegate consistencyChecker)
		{
			var definition = new StatementDefinition(
				type,
				nameGetter,
				formatTrue,
				formatFalse,
				formatQuestion,
				getDescriptionParameters,
				consistencyChecker);
			Statements.Definitions[definition.Type] = definition;
			return definition;
		}

		public static StatementDefinition<StatementT, ModuleT, LanguageStatementsT, PartT> RegisterStatement<StatementT, ModuleT, LanguageStatementsT, PartT>(
			Func<PartT, String> partGetter,
			Func<StatementT, IDictionary<String, IKnowledge>> getDescriptionParameters,
			StatementConsistencyCheckerDelegate<StatementT> consistencyChecker)
			where StatementT : class, IStatement
			where ModuleT : ILanguageStatementsExtension<LanguageStatementsT>
			where LanguageStatementsT : ILanguageExtensionStatements<PartT>
		{
			var definition = new StatementDefinition<StatementT, ModuleT, LanguageStatementsT, PartT>(
				partGetter,
				getDescriptionParameters,
				consistencyChecker);
			Statements.Definitions[definition.Type] = definition;
			return definition;
		}

		public static QuestionDefinition RegisterQuestion(
			Type type,
			Func<ILanguage, String> nameGetter)
		{
			var definition = new QuestionDefinition(type, nameGetter);
			Questions.Definitions[definition.Type] = definition;
			return definition;
		}

		public static QuestionDefinition<QuestionT> RegisterQuestion<QuestionT>(
			Func<ILanguage, String> nameGetter)
			where QuestionT : IQuestion
		{
			var definition = new QuestionDefinition<QuestionT>(nameGetter);
			Questions.Definitions[definition.Type] = definition;
			return definition;
		}

		public static AnswerDefinition RegisterAnswer(Type type)
		{
			var definition = new AnswerDefinition(type);
			Answers.Definitions[definition.Type] = definition;
			return definition;
		}

		public static CustomStatementDefinition RegisterCustomStatement<StatementT, ModuleT, LanguageStatementsT, PartT>(
			ICollection<String> concepts,
			Func<PartT, String> partGetter)
			where StatementT : class, IStatement
			where ModuleT : ILanguageStatementsExtension<LanguageStatementsT>
			where LanguageStatementsT : ILanguageExtensionStatements<PartT>
		{
			return RegisterCustomStatement(
				typeof(StatementT).Name,
				concepts,
				language => partGetter(language.GetStatementsExtension<ModuleT, LanguageStatementsT>().TrueFormatStrings),
				language => partGetter(language.GetStatementsExtension<ModuleT, LanguageStatementsT>().FalseFormatStrings),
				language => partGetter(language.GetStatementsExtension<ModuleT, LanguageStatementsT>().QuestionFormatStrings));
		}

		public static CustomStatementDefinition RegisterCustomStatement(
			String type,
			ICollection<String> concepts,
			Func<ILanguage, String> formatTrue,
			Func<ILanguage, String> formatFalse,
			Func<ILanguage, String> formatQuestion)
		{
			var definition = new CustomStatementDefinition(
				type,
				concepts,
				formatTrue,
				formatFalse,
				formatQuestion);
			_customStatements[type] = definition;
			return definition;
		}

		public static AnswerDefinition<AnswerT> RegisterAnswer<AnswerT>()
			where AnswerT : IAnswer
		{
			var definition = new AnswerDefinition<AnswerT>();
			Answers.Definitions[definition.Type] = definition;
			return definition;
		}

		public static void Reset()
		{
			_modules = new Dictionary<String, IExtensionModule>();
			_attributes = new Repository<AttributeDefinition>();
			_statements = new Repository<StatementDefinition>();
			_customStatements = new Dictionary<String, CustomStatementDefinition>();
			_questions = new Repository<QuestionDefinition>();
			_answers = new Repository<AnswerDefinition>();
			InitializeCustomStatement();
			InitializeCustomStatementQuestion();
		}

		static Repositories()
		{
			Reset();
		}

		public static void InitializeCustomStatement()
		{
			RegisterStatement(
					typeof(Statements.CustomStatement),
					language => language.Statements.CustomStatementName,
					language => throw new NotSupportedException(),
					language => throw new NotSupportedException(),
					language => throw new NotSupportedException(),
					statement => ((Statements.CustomStatement) statement).Concepts.ToDictionary(
						p => $"#{p.Key}#",
						p => p.Value as IKnowledge),
					StatementDefinition.NoConsistencyCheck)
				.SerializeToXml(statement => new Serialization.Xml.CustomStatement((Statements.CustomStatement) statement), typeof(Serialization.Xml.CustomStatement))
				.SerializeToJson(statement => new Serialization.Json.CustomStatement((Statements.CustomStatement) statement), typeof(Serialization.Json.CustomStatement));
		}

		public static void InitializeCustomStatementQuestion()
		{
			RegisterQuestion<Questions.CustomStatementQuestion>(language => language.Questions.CustomStatementQuestionName)
				.SerializeToXml(question => new Serialization.Xml.CustomStatementQuestion(question))
				.SerializeToJson(question => new Serialization.Json.CustomStatementQuestion(question));
		}
	}
}
