using System;
using System.Xml.Serialization;

using AabSemantics.Metadata;

namespace AabSemantics.Serialization.Xml
{
	/// <summary>
	/// Base XML surrogate of a statement.
	/// <para>
	/// Mind the direction of the names here, which is the reverse of
	/// <see cref="Attribute"/>: <see cref="Load"/> converts a statement <em>into</em> its
	/// surrogate, and <see cref="Save"/> restores a statement <em>from</em> one.
	/// </para>
	/// </summary>
	[XmlType]
	public abstract class Statement
	{
		#region Properties

		/// <summary>Identifier of the statement.</summary>
		[XmlAttribute]
		public String ID
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		protected Statement()
		{ }

		/// <summary>Copies the identifier from a statement.</summary>
		/// <param name="statement">Statement being converted.</param>
		protected Statement(IStatement statement)
		{
			ID = statement.ID;
		}

		#endregion

		/// <summary>Converts a statement into its surrogate, using the type's registered metadata.</summary>
		/// <param name="statement">Statement to convert.</param>
		/// <returns>The surrogate, ready to be serialized.</returns>
		/// <exception cref="NotSupportedException">The statement's type is not registered.</exception>
		public static Statement Load(IStatement statement)
		{
			var definition = Repositories.Statements.Definitions.GetSuitable(statement);
			return definition.GetSerializationSettings<StatementXmlSerializationSettings>().GetXml(statement);
		}

		/// <summary>Restores the statement from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <returns>A newly created statement.</returns>
		public abstract IStatement Save(ConceptIdResolver conceptIdResolver);

		/// <summary>
		/// Returns the network's existing statement with this identifier, or creates a new one.
		/// Used so that a deserialized question references the stored statement instead of a copy.
		/// </summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <param name="statementIdResolver">Looks the identifier up among the network's statements.</param>
		/// <returns>The existing statement, or a newly created one.</returns>
		public IStatement SaveOrReuse(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver)
		{
			IStatement result;
			return statementIdResolver.TryGetStatement(ID, out result)
				? result
				: Save(conceptIdResolver);
		}
	}

	/// <summary>XML surrogate of one concrete statement type.</summary>
	/// <typeparam name="StatementT">Statement type represented.</typeparam>
	[XmlType]
	public abstract class Statement<StatementT> : Statement
		where StatementT : IStatement
	{
		#region Constructors

		/// <summary>Creates an empty surrogate, as required by the XML serializer.</summary>
		protected Statement()
			: base()
		{ }

		/// <summary>Copies the identifier from a statement.</summary>
		/// <param name="statement">Statement being converted.</param>
		protected Statement(StatementT statement)
			: base(statement)
		{ }

		#endregion

		/// <summary>Restores the statement from the surrogate.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <returns>A newly created statement.</returns>
		public override IStatement Save(ConceptIdResolver conceptIdResolver)
		{
			return SaveImplementation(conceptIdResolver);
		}

		/// <summary>Restores the statement in its concrete type.</summary>
		/// <param name="conceptIdResolver">Resolves concept identifiers to concepts.</param>
		/// <returns>A newly created statement.</returns>
		protected abstract StatementT SaveImplementation(ConceptIdResolver conceptIdResolver);
	}
}
