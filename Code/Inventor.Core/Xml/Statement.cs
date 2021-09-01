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
			var definition = Repositories.Statements.StatementDefinitions.GetSuitable(statement);
			return definition.GetXml(statement);
		}

		public abstract IStatement Save(ConceptIdResolver conceptIdResolver);
	}
}
