using System;
using System.Collections.Generic;
using System.Data.Entity;

using Inventor.Semantics.Contexts;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Extensions.EF
{
	public class DbSemanticNetwork<ContextT> : ISemanticNetwork
		where ContextT : DbContext
	{
		#region Properties

		private readonly ContextT _dbContext;
		private readonly MappedCollection<IConcept> _concepts;
		private readonly MappedCollection<IStatement> _statements;

		public ILocalizedString Name
		{ get; }

		public ISemanticNetworkContext Context
		{ get; }

		public IKeyedCollection<IConcept> Concepts
		{ get { return _concepts; } }

		public IKeyedCollection<IStatement> Statements
		{ get { return _statements; } }

		public IDictionary<string, IExtensionModule> Modules
		{ get; }

		#endregion

		public DbSemanticNetwork(ILanguage language, ILocalizedString name, ContextT dbContext)
		{
			Name = name.EnsureNotNull(nameof(name));
			_dbContext = dbContext.EnsureNotNull(nameof(dbContext));
			_concepts = new MappedCollection<IConcept>();
			_statements = new MappedCollection<IStatement>();
			Modules = new Dictionary<String, IExtensionModule>();
			Context = new SystemContext(language).Instantiate(this);
		}

		public DbSemanticNetwork<ContextT> MapConcepts<EntityT>(
			DbSet<EntityT> dbSet,
			Func<EntityT, IConcept> map,
			Func<IConcept, EntityT> mapBack,
			Func<EntityT, string> getKey)
			where EntityT : class
		{
			_concepts.Map(
				dbSet,
				map,
				mapBack,
				getKey);
			return this;
		}

		public DbSemanticNetwork<ContextT> MapStatements<EntityT, StatementT>(
			DbSet<EntityT> dbSet,
			Func<EntityT, StatementT> map,
			Func<StatementT, EntityT> mapBack,
			Func<EntityT, string> getKey)
			where EntityT : class
			where StatementT : IStatement
		{
			_statements.Map(
				dbSet,
				statementEntity => map(statementEntity),
				statement => mapBack((StatementT) statement),
				getKey);
			return this;
		}
	}
}
