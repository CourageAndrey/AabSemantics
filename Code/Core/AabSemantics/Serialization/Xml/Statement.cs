using System;
using System.Xml.Serialization;

using AabSemantics.Metadata;

namespace AabSemantics.Serialization.Xml
{
	[XmlType]
	public abstract class Statement
	{
		#region Properties

		[XmlAttribute]
		public String ID
		{ get; set; }

		#endregion

		#region Constructors

		protected Statement()
		{ }

		protected Statement(IStatement statement)
		{
			ID = statement.ID;
		}

		#endregion

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
		#region Constructors

		protected Statement()
			: base()
		{ }

		protected Statement(StatementT statement)
			: base(statement)
		{ }

		#endregion

		public override IStatement Save(ConceptIdResolver conceptIdResolver)
		{
			return SaveImplementation(conceptIdResolver);
		}

		protected abstract StatementT SaveImplementation(ConceptIdResolver conceptIdResolver);
	}
}
