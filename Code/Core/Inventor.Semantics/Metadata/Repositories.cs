using System;
using System.Collections.Generic;

namespace Inventor.Semantics.Metadata
{
	public static class Repositories
	{
		public static IDictionary<String, IExtensionModule> Modules
		{
			get { return _modules; }
			set
			{
				if (value != null)
				{
					_modules = value;
				}
				else
				{
					throw new ArgumentNullException(nameof(value));
				}
			}
		}

		public static IRepository<AttributeDefinition> Attributes
		{
			get { return _attributes; }
			set
			{
				if (value != null)
				{
					_attributes = value;
				}
				else
				{
					throw new ArgumentNullException(nameof(value));
				}
			}
		}

		public static IRepository<StatementDefinition> Statements
		{
			get { return _statements; }
			set
			{
				if (value != null)
				{
					_statements = value;
				}
				else
				{
					throw new ArgumentNullException(nameof(value));
				}
			}
		}

		public static IRepository<QuestionDefinition> Questions
		{
			get { return _questions; }
			set
			{
				if (value != null)
				{
					_questions = value;
				}
				else
				{
					throw new ArgumentNullException(nameof(value));
				}
			}
		}

		public static IRepository<AnswerDefinition> Answers
		{
			get { return _answers; }
			set
			{
				if (value != null)
				{
					_answers = value;
				}
				else
				{
					throw new ArgumentNullException(nameof(value));
				}
			}
		}

		private static IDictionary<String, IExtensionModule> _modules = new Dictionary<String, IExtensionModule>();
		private static IRepository<AttributeDefinition> _attributes = new Repository<AttributeDefinition>();
		private static IRepository<StatementDefinition> _statements = new Repository<StatementDefinition>();
		private static IRepository<QuestionDefinition> _questions = new Repository<QuestionDefinition>();
		private static IRepository<AnswerDefinition> _answers = new Repository<AnswerDefinition>();

		public static AttributeDefinition RegisterAttribute(
			Type type,
			IAttribute value,
			Func<ILanguage, String> attributeNameGetter)
		{
			var definition = new AttributeDefinition(type, value, attributeNameGetter);
			Attributes.Definitions[definition.Type] = definition;
			return definition;
		}

		public static AttributeDefinition<AttributeT> RegisterAttribute<AttributeT>(
			AttributeT value,
			Func<ILanguage, String> attributeNameGetter)
			where AttributeT : IAttribute
		{
			var definition = new AttributeDefinition<AttributeT>(value, attributeNameGetter);
			Attributes.Definitions[definition.Type] = definition;
			return definition;
		}

		public static StatementDefinition RegisterStatement(
			Type type,
			Func<ILanguage, String> statementNameGetter,
			StatementConsistencyCheckerDelegate consistencyChecker)
		{
			var definition = new StatementDefinition(type, statementNameGetter, consistencyChecker);
			Statements.Definitions[definition.Type] = definition;
			return definition;
		}

		public static StatementDefinition<StatementT> RegisterStatement<StatementT>(
			Func<ILanguage, String> statementNameGetter,
			StatementConsistencyCheckerDelegate<StatementT> consistencyChecker)
			where StatementT : IStatement
		{
			var definition = new StatementDefinition<StatementT>(statementNameGetter, consistencyChecker);
			Statements.Definitions[definition.Type] = definition;
			return definition;
		}

		public static QuestionDefinition RegisterQuestion(
			Type type,
			Func<ILanguage, String> questionNameGetter)
		{
			var definition = new QuestionDefinition(type, questionNameGetter);
			Questions.Definitions[definition.Type] = definition;
			return definition;
		}

		public static QuestionDefinition<QuestionT> RegisterQuestion<QuestionT>(
			Func<ILanguage, String> questionNameGetter)
			where QuestionT : IQuestion
		{
			var definition = new QuestionDefinition<QuestionT>(questionNameGetter);
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
