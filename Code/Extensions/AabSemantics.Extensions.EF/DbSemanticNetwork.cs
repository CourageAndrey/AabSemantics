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
	/// <see cref="IRepository{T}"/> knows nothing of cancellation, so <see cref="Concepts"/> and
	/// <see cref="Statements"/> wait for the database uninterruptibly. Where that wait has to be
	/// cancellable, use the <c>...ConceptAsync</c> and <c>...StatementAsync</c> methods below
	/// instead; they reach the same tables and take a token.
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

		/// <summary>
		/// Concepts, read through the mappings registered by <see cref="MapConcepts"/>. Not
		/// cancellable; see <see cref="GetConceptsAsync"/> and its neighbours.
		/// </summary>
		public IRepository<IConcept> Concepts
		{ get { return _concepts; } }

		/// <summary>
		/// Statements, read through the mappings registered by <see cref="MapStatements"/>. Not
		/// cancellable; see <see cref="GetStatementsAsync"/> and its neighbours.
		/// </summary>
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

		#region Cancellable access to concepts

		/// <summary>Counts the concepts of every mapped table.</summary>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>Number of concepts.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<Int32> GetConceptCountAsync(CancellationToken cancellationToken = default)
		{
			return _concepts.GetCountAsync(cancellationToken);
		}

		/// <summary>Reads the concepts of every mapped table.</summary>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>All mapped concepts.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<ICollection<IConcept>> GetConceptsAsync(CancellationToken cancellationToken = default)
		{
			return _concepts.GetAllItemsAsync(cancellationToken);
		}

		/// <summary>Lists the identifiers of every mapped concept.</summary>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>All concept keys currently in use.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<ICollection<String>> GetConceptKeysAsync(CancellationToken cancellationToken = default)
		{
			return _concepts.GetKeysAsync(cancellationToken);
		}

		/// <summary>Looks a concept up by key.</summary>
		/// <param name="key">Identifier of the wanted concept.</param>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>The matching concept, or <c>null</c> when nothing matched.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<IConcept> GetConceptAsync(String key, CancellationToken cancellationToken = default)
		{
			return _concepts.GetItemAsync(key, cancellationToken);
		}

		/// <summary>Determines whether a concept with the given key is mapped.</summary>
		/// <param name="key">Identifier to look for.</param>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns><c>true</c> if such a concept exists.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<Boolean> ContainsConceptAsync(String key, CancellationToken cancellationToken = default)
		{
			return _concepts.ContainsAsync(key, cancellationToken);
		}

		/// <summary>Looks a concept up without throwing when it is absent.</summary>
		/// <param name="key">Identifier of the wanted concept.</param>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>A pair whose key reports success and whose value holds the concept.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<KeyValuePair<Boolean, IConcept>> TryGetConceptAsync(String key, CancellationToken cancellationToken = default)
		{
			return _concepts.TryGetValueAsync(key, cancellationToken);
		}

		/// <summary>Stages a concept; it reaches the database when <see cref="SaveChangesAsync"/> is called.</summary>
		/// <param name="concept">Concept to store.</param>
		/// <param name="cancellationToken">Cancels the call before the concept is staged.</param>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task AddConceptAsync(IConcept concept, CancellationToken cancellationToken = default)
		{
			return _concepts.AddAsync(concept, cancellationToken);
		}

		/// <summary>Stages a concept's deletion; it reaches the database when <see cref="SaveChangesAsync"/> is called.</summary>
		/// <param name="concept">Concept to remove.</param>
		/// <param name="cancellationToken">Cancels waiting for the database; nothing is staged then.</param>
		/// <returns><c>true</c> when some mapping found it.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="concept"/> is <c>null</c>.</exception>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<Boolean> RemoveConceptAsync(IConcept concept, CancellationToken cancellationToken = default)
		{
			return _concepts.RemoveAsync(concept, cancellationToken);
		}

		/// <summary>
		/// Stages the emptying of every table mapped as a source of concepts. Cancelling leaves the
		/// tables visited so far staged for emptying; <see cref="SaveChangesAsync"/> would still
		/// write those deletions.
		/// </summary>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task ClearConceptsAsync(CancellationToken cancellationToken = default)
		{
			return _concepts.ClearAsync(cancellationToken);
		}

		#endregion

		#region Cancellable access to statements

		/// <summary>Counts the statements of every mapped table.</summary>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>Number of statements.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<Int32> GetStatementCountAsync(CancellationToken cancellationToken = default)
		{
			return _statements.GetCountAsync(cancellationToken);
		}

		/// <summary>Reads the statements of every mapped table.</summary>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>All mapped statements.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<ICollection<IStatement>> GetStatementsAsync(CancellationToken cancellationToken = default)
		{
			return _statements.GetAllItemsAsync(cancellationToken);
		}

		/// <summary>Lists the identifiers of every mapped statement.</summary>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>All statement keys currently in use.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<ICollection<String>> GetStatementKeysAsync(CancellationToken cancellationToken = default)
		{
			return _statements.GetKeysAsync(cancellationToken);
		}

		/// <summary>Looks a statement up by key.</summary>
		/// <param name="key">Identifier of the wanted statement.</param>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>The matching statement, or <c>null</c> when nothing matched.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<IStatement> GetStatementAsync(String key, CancellationToken cancellationToken = default)
		{
			return _statements.GetItemAsync(key, cancellationToken);
		}

		/// <summary>Determines whether a statement with the given key is mapped.</summary>
		/// <param name="key">Identifier to look for.</param>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns><c>true</c> if such a statement exists.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<Boolean> ContainsStatementAsync(String key, CancellationToken cancellationToken = default)
		{
			return _statements.ContainsAsync(key, cancellationToken);
		}

		/// <summary>Looks a statement up without throwing when it is absent.</summary>
		/// <param name="key">Identifier of the wanted statement.</param>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <returns>A pair whose key reports success and whose value holds the statement.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<KeyValuePair<Boolean, IStatement>> TryGetStatementAsync(String key, CancellationToken cancellationToken = default)
		{
			return _statements.TryGetValueAsync(key, cancellationToken);
		}

		/// <summary>Stages a statement; it reaches the database when <see cref="SaveChangesAsync"/> is called.</summary>
		/// <param name="statement">Statement to store.</param>
		/// <param name="cancellationToken">Cancels the call before the statement is staged.</param>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task AddStatementAsync(IStatement statement, CancellationToken cancellationToken = default)
		{
			return _statements.AddAsync(statement, cancellationToken);
		}

		/// <summary>Stages a statement's deletion; it reaches the database when <see cref="SaveChangesAsync"/> is called.</summary>
		/// <param name="statement">Statement to remove.</param>
		/// <param name="cancellationToken">Cancels waiting for the database; nothing is staged then.</param>
		/// <returns><c>true</c> when some mapping found it.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="statement"/> is <c>null</c>.</exception>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<Boolean> RemoveStatementAsync(IStatement statement, CancellationToken cancellationToken = default)
		{
			return _statements.RemoveAsync(statement, cancellationToken);
		}

		/// <summary>
		/// Stages the emptying of every table mapped as a source of statements. Cancelling leaves the
		/// tables visited so far staged for emptying; <see cref="SaveChangesAsync"/> would still
		/// write those deletions.
		/// </summary>
		/// <param name="cancellationToken">Cancels waiting for the database.</param>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task ClearStatementsAsync(CancellationToken cancellationToken = default)
		{
			return _statements.ClearAsync(cancellationToken);
		}

		#endregion
	}
}
