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
			Serialization.Xml.Attribute xml)
		{
			Attributes.Define(new AttributeDefinition(type, value, attributeNameGetter, xml));
		}

		public static void RegisterAttribute<AttributeT>(
			AttributeT value,
			Func<ILanguage, String> attributeNameGetter,
			Serialization.Xml.Attribute xml)
			where AttributeT : IAttribute
		{
			Attributes.Define(new AttributeDefinition<AttributeT>(attributeNameGetter, value, xml));
		}

		public static void RegisterAttribute<AttributeT, XmlT>(
			AttributeT value,
			Func<ILanguage, String> attributeNameGetter,
			XmlT xml)
			where AttributeT : IAttribute
			where XmlT : Serialization.Xml.Attribute
		{
			Attributes.Define(new AttributeDefinition<AttributeT, XmlT>(attributeNameGetter, value, xml));
		}

		public static void RegisterStatement(
			Type type,
			Func<ILanguage, String> statementNameGetter,
			Func<IStatement, Serialization.Xml.Statement> statementXmlGetter,
			Func<IStatement, Serialization.Json.Statement> statementJsonGetter,
			Type xmlType,
			Type jsonType,
			StatementConsistencyCheckerDelegate consistencyChecker)
		{
			Statements.Define(new StatementDefinition(type, statementNameGetter, statementXmlGetter, statementJsonGetter, xmlType, jsonType, consistencyChecker));
		}

		public static void RegisterStatement<StatementT>(
			Func<ILanguage, String> statementNameGetter,
			Func<StatementT, Serialization.Xml.Statement> statementXmlGetter,
			Func<StatementT, Serialization.Json.Statement> statementJsonGetter,
			Type xmlType,
			Type jsonType,
			StatementConsistencyCheckerDelegate<StatementT> consistencyChecker)
			where StatementT : IStatement
		{
			Statements.Define(new StatementDefinition<StatementT>(statementNameGetter, statementXmlGetter, statementJsonGetter, xmlType, jsonType, consistencyChecker));
		}

		public static void RegisterStatement<StatementT, XmlT, JsonT>(
			Func<ILanguage, String> statementNameGetter,
			Func<StatementT, XmlT> statementXmlGetter,
			Func<StatementT, JsonT> statementJsonGetter,
			StatementConsistencyCheckerDelegate<StatementT> consistencyChecker)
			where StatementT : IStatement
			where XmlT : Serialization.Xml.Statement
			where JsonT : Serialization.Json.Statement
		{
			Statements.Define(new StatementDefinition<StatementT, XmlT, JsonT>(statementNameGetter, statementXmlGetter, statementJsonGetter, consistencyChecker));
		}

		public static void RegisterQuestion(
			Type type,
			Func<ILanguage, String> questionNameGetter,
			Func<IQuestion, Serialization.Xml.Question> questionXmlGetter,
			Func<IQuestion, Serialization.Json.Question> questionJsonGetter,
			Type xmlType,
			Type jsonType)
		{
			Questions.Define(new QuestionDefinition(type, questionNameGetter, questionXmlGetter, questionJsonGetter, xmlType, jsonType));
		}

		public static void RegisterQuestion<QuestionT>(
			Func<ILanguage, String> questionNameGetter,
			Func<QuestionT, Serialization.Xml.Question> questionXmlGetter,
			Func<QuestionT, Serialization.Json.Question> questionJsonGetter,
			Type xmlType,
			Type jsonType)
			where QuestionT : IQuestion
		{
			Questions.Define(new QuestionDefinition<QuestionT>(questionNameGetter, questionXmlGetter, questionJsonGetter, xmlType, jsonType));
		}

		public static void RegisterQuestion<QuestionT, XmlT, JsonT>(
			Func<ILanguage, String> questionNameGetter,
			Func<QuestionT, XmlT> questionXmlGetter,
			Func<QuestionT, JsonT> questionJsonGetter)
			where QuestionT : IQuestion
			where XmlT : Serialization.Xml.Question
			where JsonT : Serialization.Json.Question
		{
			Questions.Define(new QuestionDefinition<QuestionT, XmlT, JsonT>(questionNameGetter, questionXmlGetter, questionJsonGetter));
		}

		public static void RegisterAnswer(
			Type type,
			Func<IAnswer, ILanguage, Serialization.Xml.Answer> answerXmlGetter,
			Func<IAnswer, ILanguage, Serialization.Json.Answer> answerJsonGetter,
			Type xmlType,
			Type jsonType)
		{
			Answers.Define(new AnswerDefinition(type, answerXmlGetter, answerJsonGetter, xmlType, jsonType));
		}

		public static void RegisterAnswer<AnswerT>(
			Func<AnswerT, ILanguage, Serialization.Xml.Answer> answerXmlGetter,
			Func<AnswerT, ILanguage, Serialization.Json.Answer> answerJsonGetter,
			Type xmlType,
			Type jsonType)
			where AnswerT : IAnswer
		{
			Answers.Define(new AnswerDefinition<AnswerT>(answerXmlGetter, answerJsonGetter, xmlType, jsonType));
		}

		public static void RegisterAnswer<AnswerT, XmlT, JsonT>(
			Func<AnswerT, ILanguage, XmlT> answerXmlGetter,
			Func<AnswerT, ILanguage, JsonT> answerJsonGetter)
			where AnswerT : IAnswer
			where XmlT : Serialization.Xml.Answer
			where JsonT : Serialization.Json.Answer
		{
			Answers.Define(new AnswerDefinition<AnswerT, XmlT, JsonT>(answerXmlGetter, answerJsonGetter));
		}
	}
}
