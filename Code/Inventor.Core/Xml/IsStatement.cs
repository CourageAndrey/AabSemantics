using System;
using System.Xml.Serialization;

namespace Inventor.Core.Xml
{
	[XmlType]
	public class IsStatement : Statement
	{
		#region Properties

		[XmlAttribute]
		public String Ancestor
		{ get; set; }

		[XmlAttribute]
		public String Descendant
		{ get; set; }

		#endregion

		#region Constructors

		public IsStatement()
		{ }

		public IsStatement(Statements.IsStatement statement, LoadIdResolver conceptIdResolver)
		{
			Ancestor = conceptIdResolver.GetConceptId(statement.Ancestor);
			Descendant = conceptIdResolver.GetConceptId(statement.Descendant);
		}

		#endregion

		public override IStatement Save(SaveIdResolver conceptIdResolver)
		{
			return new Statements.IsStatement(conceptIdResolver.GetConceptById(Ancestor), conceptIdResolver.GetConceptById(Descendant));
		}
	}
}
