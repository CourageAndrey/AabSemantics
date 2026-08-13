using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Statements;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Classification.Statements
{
	/// <summary>
	/// The "is a" relation: one concept is a kind of another. Implementing
	/// <see cref="IParentChild{T}"/> is what makes it traversable by the engine's hierarchy helpers.
	/// </summary>
	public class IsStatement : Statement<IsStatement>, IParentChild<IConcept>
	{
		#region Properties

		/// <summary>The more general concept.</summary>
		public IConcept Ancestor
		{ get; private set; }

		/// <summary>The more specific concept.</summary>
		public IConcept Descendant
		{ get; private set; }

		/// <summary>Same as <see cref="Ancestor"/>, under the generic hierarchy naming.</summary>
		public IConcept Parent
		{ get { return Ancestor; } }

		/// <summary>Same as <see cref="Descendant"/>, under the generic hierarchy naming.</summary>
		public IConcept Child
		{ get { return Descendant; } }

		#endregion

		/// <summary>Creates an "is a" statement.</summary>
		/// <param name="id">Identifier; a GUID is generated when null or empty.</param>
		/// <param name="ancestor">The more general concept.</param>
		/// <param name="descendant">The more specific concept.</param>
		/// <exception cref="ArgumentNullException"><paramref name="ancestor"/> or <paramref name="descendant"/> is <c>null</c>.</exception>
		public IsStatement(String id, IConcept ancestor, IConcept descendant)
			: base(
				id,
				new Func<ILanguage, String>(language => language.GetStatementsExtension<ILanguageClassificationModule, ILanguageStatements>().Names.Classification),
				new Func<ILanguage, String>(language => language.GetStatementsExtension<ILanguageClassificationModule, ILanguageStatements>().Hints.Classification))
		{
			Update(id, ancestor, descendant);
		}

		/// <summary>Reassigns the identifier and both related concepts.</summary>
		/// <param name="id">New identifier; a GUID is generated when null or empty.</param>
		/// <param name="ancestor">The more general concept.</param>
		/// <param name="descendant">The more specific concept.</param>
		/// <exception cref="ArgumentNullException">Either concept is <c>null</c>.</exception>
		public void Update(String id, IConcept ancestor, IConcept descendant)
		{
			Update(id);
			Ancestor = ancestor.EnsureNotNull(nameof(ancestor));
			Descendant = descendant.EnsureNotNull(nameof(descendant));
		}

		/// <summary>Returns both related concepts.</summary>
		/// <returns>The ancestor followed by the descendant.</returns>
		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return Ancestor;
			yield return Descendant;
		}

		#region Consistency checking

		/// <summary>Compares both related concepts by reference.</summary>
		/// <param name="other">Statement to compare with; may be <c>null</c>.</param>
		/// <returns><c>true</c> if both relate the same pair of concepts.</returns>
		public override System.Boolean Equals(IsStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.Ancestor == Ancestor &&
						other.Descendant == Descendant;
			}
			else return false;
		}

		/// <summary>
		/// Reports whether this statement is free of a cycle: it searches for a path leading from
		/// the descendant back down to the ancestor, which would mean the two are each other's kind.
		/// </summary>
		/// <param name="statements">Statements to search, normally all "is a" statements of the network.</param>
		/// <returns><c>true</c> when no such reverse path exists, i.e. the hierarchy stays acyclic.</returns>
		public async Task<System.Boolean> CheckCyclicAsync(IEnumerable<IsStatement> statements)
		{
			var path = await statements.FindPathAsync(typeof(IsStatement), Child, Parent);
			return !path.Any();
		}

		#endregion
	}

	/// <summary>Blocking wrappers over <see cref="IsStatement"/>.</summary>
	public static class IsStatementExtensions
	{
		/// <summary>Blocking counterpart of <see cref="IsStatement.CheckCyclicAsync"/>.</summary>
		/// <param name="statement">Statement to check.</param>
		/// <param name="statements">Statements to search.</param>
		/// <returns><c>true</c> when the hierarchy stays acyclic.</returns>
		public static System.Boolean CheckCyclic(this IsStatement statement, IEnumerable<IsStatement> statements)
		{
			return TaskHelper.AwaitDetached(() => statement.CheckCyclicAsync(statements));
		}
	}
}
