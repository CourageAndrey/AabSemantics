using System;

using Inventor.Semantics.Metadata;

namespace Inventor.Semantics.Serialization.Json
{
	[Serializable]
	public abstract class Statement
	{
		public String ID
		{ get; set; }

		public static Statement Load(IStatement statement)
		{
			var definition = Repositories.Statements.Definitions.GetSuitable(statement);
			return definition.GetJson(statement);
		}

		public abstract IStatement Save(ConceptIdResolver conceptIdResolver);
	}

	[Serializable]
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
