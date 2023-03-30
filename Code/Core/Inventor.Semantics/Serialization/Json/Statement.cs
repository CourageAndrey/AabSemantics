using System;
using System.Runtime.Serialization;

using Inventor.Semantics.Metadata;

namespace Inventor.Semantics.Serialization.Json
{
	[DataContract]
	public abstract class Statement
	{
		#region Properties

		[DataMember]
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
			return definition.GetJsonSerializationSettings<StatementJsonSerializationSettings>().GetJson(statement);
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

	[DataContract]
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
