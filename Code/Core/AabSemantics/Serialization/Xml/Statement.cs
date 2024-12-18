using System;
using System.Xml.Serialization;

using AabSemantics.Metadata;

namespace AabSemantics.Serialization.Xml
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
			return definition.GetSerializationSettings<StatementXmlSerializationSettings>().GetXml(statement);
		}

		public abstract IStatement Save(ConceptIdResolver conceptIdResolver);

		public IStatement SaveOrReuse(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			IStatement result;
			return statementIdResolver.TryGetStatement(ID, out result)
				? result
				: Save(conceptIdResolver);
		}
	}

	[XmlType]
	public abstract class Statement<StatementT> : Statement
		where StatementT : IStatement
	{
		public override IStatement Save(ConceptIdResolver conceptIdResolver)
		{
			return SaveImplementation(conceptIdResolver);
		}

		protected abstract StatementT SaveImplementation(ConceptIdResolver conceptIdResolver);
	}
}
