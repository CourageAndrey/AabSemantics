using System;
using System.Collections.Generic;

using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Metadata
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

		private static IDictionary<String, IExtensionModule> _modules = new Dictionary<String, IExtensionModule>();
		private static IRepository<AttributeDefinition> _attributes = new Repository<AttributeDefinition>();
		private static IRepository<StatementDefinition> _statements = new Repository<StatementDefinition>();
		private static IRepository<QuestionDefinition> _questions = new Repository<QuestionDefinition>();
		private static IRepository<AnswerDefinition> _answers = new Repository<AnswerDefinition>();

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
			StatementConsistencyCheckerDelegate consistencyChecker)
		{
			var definition = new StatementDefinition(type, nameGetter, consistencyChecker);
			Statements.Definitions[definition.Type] = definition;
			return definition;
		}

		public static StatementDefinition<StatementT> RegisterStatement<StatementT>(
			Func<ILanguage, String> nameGetter,
			StatementConsistencyCheckerDelegate<StatementT> consistencyChecker)
			where StatementT : IStatement
		{
			var definition = new StatementDefinition<StatementT>(nameGetter, consistencyChecker);
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

		public static AnswerDefinition RegisterAnswer(
			Type type)
		{
			var definition = new AnswerDefinition(type);
			Answers.Definitions[definition.Type] = definition;
			return definition;
		}

		public static AnswerDefinition<AnswerT> RegisterAnswer<AnswerT>()
			where AnswerT : IAnswer
		{
			var definition = new AnswerDefinition<AnswerT>();
			Answers.Definitions[definition.Type] = definition;
			return definition;
		}
	}
}
