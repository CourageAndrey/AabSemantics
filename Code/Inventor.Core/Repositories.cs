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
	}
}
