using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

using AabSemantics.Contexts;
using AabSemantics.Utils;

namespace AabSemantics.Extensions.EF
{
	/// <summary>
	/// Semantic network backed by an Entity Framework context instead of memory. Concepts and
	/// statements are read from and written to the mapped <see cref="DbSet{TEntity}"/>s.
	/// Writes are deferred: adding and removing knowledge only stages the change, and
	/// <see cref="SaveChangesAsync"/> commits everything staged so far in a single transaction.
	/// Until then the changes are visible through this network alone.
	/// <para>
	/// Waiting for the database can be cut short throughout: every <see cref="IRepository{T}"/>
	/// member of <see cref="Concepts"/> and <see cref="Statements"/> takes a cancellation token.
	/// Reading a collection whole is the one thing the interface cannot do cancellably, as it
	/// offers no token to <see cref="System.Collections.Generic.IEnumerable{T}.GetEnumerator"/>;
	/// <see cref="GetConceptsAsync"/> and <see cref="GetStatementsAsync"/> fill that gap.
	/// </para>
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
				_dbContext,
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
				_dbContext,
				dbSet,
				statementEntity => map(statementEntity),
				statement => mapBack((StatementT) statement),
				getKey);
			return this;
		}

		/// <summary>
		/// Writes every change staged since the last save. Entity Framework sends them as one
		/// transaction, so either the whole batch reaches the database or none of it does.
		/// </summary>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>Number of affected rows.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public async Task<Int32> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		/// <summary>
		/// Reads every mapped concept, pending changes included. Cancellable alternative to
		/// enumerating <see cref="Concepts"/>.
		/// </summary>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>All mapped concepts.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<ICollection<IConcept>> GetConceptsAsync(CancellationToken cancellationToken = default)
		{
			return _concepts.GetAllItemsAsync(cancellationToken);
		}

		/// <summary>
		/// Reads every mapped statement, pending changes included. Cancellable alternative to
		/// enumerating <see cref="Statements"/>.
		/// </summary>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>All mapped statements.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<ICollection<IStatement>> GetStatementsAsync(CancellationToken cancellationToken = default)
		{
			return _statements.GetAllItemsAsync(cancellationToken);
		}
	}
}
