using System;
using System.Runtime.Serialization;

using Inventor.Semantics.Metadata;

namespace Inventor.Semantics.Serialization.Json
{
	[DataContract]
	public abstract class Statement
	{
		[DataMember]
		public String ID
		{ get; set; }

		public static Statement Load(IStatement statement)
		{
			var definition = Repositories.Statements.Definitions.GetSuitable(statement);
			return definition.GetJsonSerializationSettings<StatementJsonSerializationSettings>().GetJson(statement);
		}

		public abstract IStatement Save(ConceptIdResolver conceptIdResolver);
	}

	[DataContract]
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
