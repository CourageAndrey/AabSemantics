using System;

namespace Inventor.Core
{
	public static class Repositories
	{
		public static IAttributeRepository Attributes
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

		public static IStatementRepository Statements
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

		public static IQuestionRepository Questions
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

		private static IAttributeRepository _attributes = new Base.AttributeRepository();
		private static IStatementRepository _statements = new Base.StatementRepository();
		private static IQuestionRepository _questions = new Base.QuestionRepository();
	}
}
