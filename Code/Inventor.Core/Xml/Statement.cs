using System;
using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[XmlType]
	public abstract class Statement
	{
		public static Statement Load(IStatement statement, LoadIdResolver conceptIdResolver)
		{
			if (statement is Statements.ComparisonStatement)
			{
				return new ComparisonStatement(statement as Statements.ComparisonStatement, conceptIdResolver);
			}
			else if (statement is Statements.GroupStatement)
			{
				return new GroupStatement(statement as Statements.GroupStatement, conceptIdResolver);
			}
			else if (statement is Statements.HasPartStatement)
			{
				return new HasPartStatement(statement as Statements.HasPartStatement, conceptIdResolver);
			}
			else if (statement is Statements.HasSignStatement)
			{
				return new HasSignStatement(statement as Statements.HasSignStatement, conceptIdResolver);
			}
			else if (statement is Statements.IsStatement)
			{
				return new IsStatement(statement as Statements.IsStatement, conceptIdResolver);
			}
			else if (statement is Statements.ProcessesStatement)
			{
				return new ProcessesStatement(statement as Statements.ProcessesStatement, conceptIdResolver);
			}
			else if (statement is Statements.SignValueStatement)
			{
				return new SignValueStatement(statement as Statements.SignValueStatement, conceptIdResolver);
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		public abstract IStatement Save(SaveIdResolver conceptIdResolver);
	}
}
