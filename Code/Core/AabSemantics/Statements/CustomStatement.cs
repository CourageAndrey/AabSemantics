using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Metadata;

namespace AabSemantics.Statements
{
	/// <summary>
	/// Statement of a kind declared at run time. All custom kinds share this single class and are
	/// told apart by <see cref="Type"/>, with their roles held in a name-to-concept map instead of
	/// typed properties.
	/// </summary>
	public class CustomStatement : Statement<CustomStatement>
	{
		#region Properties

		/// <summary>Identifier of the declared statement kind.</summary>
		public String Type
		{ get { return _definition.Kind; } }

		/// <summary>Concepts filling the kind's roles, keyed by role name.</summary>
		public IDictionary<String, IConcept> Concepts
		{ get; private set; }

		private readonly CustomStatementDefinition _definition;

		#endregion

		/// <summary>Creates a statement of a previously declared custom kind.</summary>
		/// <param name="id">Identifier; a GUID is generated when null or empty.</param>
		/// <param name="type">Identifier of the declared kind.</param>
		/// <param name="concepts">Concepts filling the roles; an empty map when <c>null</c>.</param>
		/// <exception cref="KeyNotFoundException">The kind has not been registered.</exception>
		public CustomStatement(
			String id,
			String type,
			IDictionary<String, IConcept> concepts = null)
			: base(id, CustomStatementDefinition.GetStatementName, CustomStatementDefinition.GetStatementName)
		{
			_definition = Repositories.CustomStatements[type];

			Update(id, concepts);
		}

		/// <summary>Reassigns the identifier and replaces the role concepts.</summary>
		/// <param name="id">New identifier; a GUID is generated when null or empty.</param>
		/// <param name="concepts">New role concepts; copied into the statement.</param>
		public void Update(String id, IDictionary<String, IConcept> concepts)
		{
			Update(id);

			Concepts = new Dictionary<string, IConcept>(concepts ?? new Dictionary<String, IConcept>());
		}

		/// <summary>Returns the concepts filling the statement's roles.</summary>
		/// <returns>Concepts participating in the statement.</returns>
		public override IEnumerable<IConcept> GetChildConcepts()
		{
			return Concepts.Values;
		}

		#region Consistency checking

		/// <summary>Compares kind and role concepts, the latter order-sensitively.</summary>
		/// <param name="other">Statement to compare with; may be <c>null</c>.</param>
		/// <returns><c>true</c> if both assert the same thing.</returns>
		public override Boolean Equals(CustomStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.Type == Type &&
						other.Concepts.SequenceEqual(Concepts);
			}
			else return false;
		}

		#endregion
	}
}
