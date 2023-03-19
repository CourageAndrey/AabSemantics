using System;
using System.Xml.Serialization;

using Inventor.Semantics.Metadata;

namespace Inventor.Semantics.Serialization.Xml
{
	[XmlType]
	public abstract class Statement
	{
		[XmlAttribute]
		public String ID
		{ get; set; }

		public static Statement Load(IStatement statement)
		{
			var definition = Repositories.Statements.Definitions.GetSuitable(statement);
			return definition.GetXml(statement);
		}

		public abstract IStatement Save(ConceptIdResolver conceptIdResolver);
	}
}
