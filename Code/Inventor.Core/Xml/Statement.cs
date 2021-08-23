using System;
using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[XmlType]
	public abstract class Statement
	{
		[XmlAttribute]
		public String ID
		{ get; set; }

		public static Statement Load(IStatement statement)
		{
			if (statement is Statements.ComparisonStatement)
			{
				return new ComparisonStatement(statement as Statements.ComparisonStatement);
			}
			else if (statement is Statements.GroupStatement)
			{
				return new GroupStatement(statement as Statements.GroupStatement);
			}
			else if (statement is Statements.HasPartStatement)
			{
				return new HasPartStatement(statement as Statements.HasPartStatement);
			}
			else if (statement is Statements.HasSignStatement)
			{
				return new HasSignStatement(statement as Statements.HasSignStatement);
			}
			else if (statement is Statements.IsStatement)
			{
				return new IsStatement(statement as Statements.IsStatement);
			}
			else if (statement is Statements.ProcessesStatement)
			{
				return new ProcessesStatement(statement as Statements.ProcessesStatement);
			}
			else if (statement is Statements.SignValueStatement)
			{
				return new SignValueStatement(statement as Statements.SignValueStatement);
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		public abstract IStatement Save(ConceptIdResolver conceptIdResolver);
	}
}
