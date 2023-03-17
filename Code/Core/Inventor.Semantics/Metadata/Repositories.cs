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

		public static void RegisterAttribute(
			Type type,
			IAttribute value,
			Func<ILanguage, String> attributeNameGetter,
			Xml.Attribute xml)
		{
			Attributes.Define(new AttributeDefinition(type, value, attributeNameGetter, xml));
		}

		public static void RegisterAttribute<AttributeT>(
			AttributeT value,
			Func<ILanguage, String> attributeNameGetter,
			Xml.Attribute xml)
			where AttributeT : IAttribute
		{
			Attributes.Define(new AttributeDefinition<AttributeT>(attributeNameGetter, value, xml));
		}

		public static void RegisterAttribute<AttributeT, XmlT>(
			AttributeT value,
			Func<ILanguage, String> attributeNameGetter,
			XmlT xml)
			where AttributeT : IAttribute
			where XmlT : Xml.Attribute
		{
			Attributes.Define(new AttributeDefinition<AttributeT, XmlT>(attributeNameGetter, value, xml));
		}

		public static void RegisterStatement(
			Type type,
			Func<ILanguage, String> statementNameGetter,
			Func<IStatement, Xml.Statement> statementXmlGetter,
			Type xmlType,
			StatementConsistencyCheckerDelegate consistencyChecker)
		{
			Statements.Define(new StatementDefinition(type, statementNameGetter, statementXmlGetter, xmlType, consistencyChecker));
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

		public static void RegisterStatement<StatementT, XmlT>(
			Func<ILanguage, String> statementNameGetter,
			Func<StatementT, XmlT> statementXmlGetter,
			StatementConsistencyCheckerDelegate<StatementT> consistencyChecker)
			where StatementT : IStatement
			where XmlT : Xml.Statement
		{
			Statements.Define(new StatementDefinition<StatementT, XmlT>(statementNameGetter, statementXmlGetter, consistencyChecker));
		}

		public static void RegisterQuestion(
			Type type,
			Func<ILanguage, String> questionNameGetter,
			Func<IQuestion, Xml.Question> questionXmlGetter,
			Type xmlType)
		{
			Questions.Define(new QuestionDefinition(type, questionNameGetter, questionXmlGetter, xmlType));
		}

		public static void RegisterQuestion<QuestionT>(
			Func<ILanguage, String> questionNameGetter,
			Func<QuestionT, Xml.Question> questionXmlGetter,
			Type xmlType)
			where QuestionT : IQuestion
		{
			Questions.Define(new QuestionDefinition<QuestionT>(questionNameGetter, questionXmlGetter, xmlType));
		}

		public static void RegisterQuestion<QuestionT, XmlT>(
			Func<ILanguage, String> questionNameGetter,
			Func<QuestionT, XmlT> questionXmlGetter)
			where QuestionT : IQuestion
			where XmlT : Xml.Question
		{
			Questions.Define(new QuestionDefinition<QuestionT, XmlT>(questionNameGetter, questionXmlGetter));
		}

		public static void RegisterAnswer(
			Type type,
			Func<IAnswer, ILanguage, Xml.Answer> answerXmlGetter,
			Type xmlType)
		{
			Answers.Define(new AnswerDefinition(type, answerXmlGetter, xmlType));
		}

		public static void RegisterAnswer<AnswerT>(
			Func<AnswerT, ILanguage, Xml.Answer> answerXmlGetter,
			Type xmlType)
			where AnswerT : IAnswer
		{
			Answers.Define(new AnswerDefinition<AnswerT>(answerXmlGetter, xmlType));
		}

		public static void RegisterAnswer<AnswerT, XmlT>(
			Func<AnswerT, ILanguage, XmlT> answerXmlGetter)
			where AnswerT : IAnswer
			where XmlT : Xml.Answer
		{
			Answers.Define(new AnswerDefinition<AnswerT, XmlT>(answerXmlGetter));
		}
	}
}
