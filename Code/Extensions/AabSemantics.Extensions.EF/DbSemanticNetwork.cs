using System;
using System.Collections.Generic;
using System.Data.Entity;

using AabSemantics.Contexts;
using AabSemantics.Utils;

namespace AabSemantics.Extensions.EF
{
	/// <summary>
	/// Semantic network backed by an Entity Framework context instead of memory. Concepts and
	/// statements are read from and written to the mapped <see cref="DbSet{TEntity}"/>s.
	/// </summary>
	/// <typeparam name="ContextT">Entity Framework context type.</typeparam>
	public class DbSemanticNetwork<ContextT> : ISemanticNetwork
		where ContextT : DbContext
	{
		#region Properties

		private readonly ContextT _dbContext;
		private readonly MappedCollection<IConcept> _concepts;
		private readonly MappedCollection<IStatement> _statements;

		/// <summary>Localized name of the network.</summary>
		public ILocalizedString Name
		{ get; }

		/// <summary>Context the network's knowledge lives in.</summary>
		public ISemanticNetworkContext Context
		{ get; }

		/// <summary>Concepts, read through the mappings registered by <see cref="MapConcepts"/>.</summary>
		public IRepository<IConcept> Concepts
		{ get { return _concepts; } }

		/// <summary>Statements, read through the mappings registered by <see cref="MapStatements"/>.</summary>
		public IRepository<IStatement> Statements
		{ get { return _statements; } }

		/// <summary>Extension modules attached to the network, keyed by module name.</summary>
		public IDictionary<string, IExtensionModule> Modules
		{ get; }

		#endregion

		/// <summary>Creates a database-backed network with no mappings yet.</summary>
		/// <param name="language">Language for text produced by the network.</param>
		/// <param name="name">Localized name of the network.</param>
		/// <param name="dbContext">Entity Framework context holding the data.</param>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> or <paramref name="dbContext"/> is <c>null</c>.</exception>
		public DbSemanticNetwork(ILanguage language, ILocalizedString name, ContextT dbContext)
		{
			Name = name.EnsureNotNull(nameof(name));
			_dbContext = dbContext.EnsureNotNull(nameof(dbContext));
			_concepts = new MappedCollection<IConcept>();
			_statements = new MappedCollection<IStatement>();
			Modules = new Dictionary<String, IExtensionModule>();
			Context = new SystemContext(language).Instantiate(this);
		}

		/// <summary>Registers a table as a source of concepts.</summary>
		/// <typeparam name="EntityT">Entity type stored in the table.</typeparam>
		/// <param name="dbSet">Table to map.</param>
		/// <param name="map">Converts an entity into a concept.</param>
		/// <param name="mapBack">Converts a concept into an entity.</param>
		/// <param name="getKey">Returns an entity's identifier.</param>
		/// <returns>The same network, to allow call chaining.</returns>
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

		/// <summary>Registers a table as a source of statements of one type.</summary>
		/// <typeparam name="EntityT">Entity type stored in the table.</typeparam>
		/// <typeparam name="StatementT">Statement type the entities represent.</typeparam>
		/// <param name="dbSet">Table to map.</param>
		/// <param name="map">Converts an entity into a statement.</param>
		/// <param name="mapBack">Converts a statement into an entity.</param>
		/// <param name="getKey">Returns an entity's identifier.</param>
		/// <returns>The same network, to allow call chaining.</returns>
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
