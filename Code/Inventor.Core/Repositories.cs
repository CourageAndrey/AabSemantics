using System;

namespace Inventor.Core
{
	public static class Repositories
	{
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

		private static IRepository<AttributeDefinition> _attributes = new Base.Repository<AttributeDefinition>();
		private static IRepository<StatementDefinition> _statements = new Base.Repository<StatementDefinition>();
		private static IRepository<QuestionDefinition> _questions = new Base.Repository<QuestionDefinition>();

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

		public static void RegisterQuestion<QuestionT>()
			where QuestionT : IQuestion
		{
			Questions.Define(new QuestionDefinition(typeof(QuestionT)));
		}
	}
}
