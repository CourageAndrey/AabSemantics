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

		public static void RegisterAttribute<AttributeT>(
			AttributeT value,
			Func<ILanguage, String> attributeNameGetter,
			Xml.Attribute xml)
			where AttributeT : IAttribute
		{
			Attributes.Define(new AttributeDefinition(typeof(AttributeT), value, attributeNameGetter, xml));
		}

		public static void RegisterStatement<StatementT>(
			Func<ILanguage, String> statementNameGetter,
			Func<StatementT, Xml.Statement> statementXmlGetter,
			Type xmlType,
			StatementConsistencyCheckerDelegate<StatementT> consistencyChecker)
			where StatementT : IStatement
		{
			Statements.Define(new StatementDefinition<StatementT>(statementNameGetter, statementXmlGetter, xmlType, consistencyChecker));
		}

		public static void RegisterQuestion<QuestionT>(
			Func<ILanguage, String> questionNameGetter,
			Func<QuestionT, Xml.Question> questionXmlGetter,
			Type xmlType)
			where QuestionT : IQuestion
		{
			Questions.Define(new QuestionDefinition<QuestionT>(questionNameGetter, questionXmlGetter, xmlType));
		}

		public static void RegisterAnswer<AnswerT>(
			Func<AnswerT, ILanguage, Xml.Answer> answerXmlGetter,
			Type xmlType)
			where AnswerT : IAnswer
		{
			Answers.Define(new AnswerDefinition<AnswerT>(answerXmlGetter, xmlType));
		}
	}
}
