using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Localization;
using AabSemantics.Statements;

namespace AabSemantics.Metadata
{
	/// <summary>
	/// Describes a statement kind declared at run time rather than by a compiled type.
	/// Every such statement shares the <see cref="CustomStatement"/> class and is told apart
	/// by its <see cref="Kind"/>, which lets a knowledge base define its own relations
	/// without a custom module.
	/// </summary>
	public class CustomStatementDefinition : StatementDefinition, INamed
	{
		#region Properties

		/// <summary>
		/// Identifier of this statement kind, unique among custom statements.
		/// </summary>
		public String Kind
		{ get; }

		/// <summary>
		/// Names of the roles this statement relates, e.g. "subject" and "object".
		/// Each instance supplies one concept per role.
		/// </summary>
		public ICollection<String> Concepts
		{ get; }

		/// <summary>Reads the shared display name used by every custom statement kind.</summary>
		internal static readonly Func<ILanguage, String> GetStatementName = language => language.Statements.CustomStatementName;

		#endregion

		/// <summary>
		/// Declares a custom statement kind. Consistency checking is not supported for these,
		/// so no check is registered.
		/// </summary>
		/// <param name="kind">Identifier of the statement kind.</param>
		/// <param name="concepts">Names of the roles the statement relates.</param>
		/// <param name="formatTrue">Selects the affirmative wording from a language.</param>
		/// <param name="formatFalse">Selects the negative wording from a language.</param>
		/// <param name="formatQuestion">Selects the interrogative wording from a language.</param>
		/// <exception cref="ArgumentNullException"><paramref name="kind"/> is null or empty, or <paramref name="concepts"/> is <c>null</c>.</exception>
		public CustomStatementDefinition(
			String kind,
			ICollection<String> concepts,
			Func<ILanguage, String> formatTrue,
			Func<ILanguage, String> formatFalse,
			Func<ILanguage, String> formatQuestion)
		: base(
			typeof(CustomStatement),
			GetStatementName,
			formatTrue,
			formatFalse,
			formatQuestion,
			statement => ((CustomStatement) statement).Concepts.ToDictionary(
				p => p.Key,
				p => p.Value as IKnowledge),
			NoConsistencyCheck)
		{
			if (!String.IsNullOrEmpty(kind))
			{
				Kind = kind;
			}
			else
			{
				throw new ArgumentNullException(nameof(kind));
			}

			Concepts = concepts ?? throw new ArgumentNullException(nameof(concepts));
		}

		/// <summary>
		/// Returns <see cref="Kind"/>.
		/// </summary>
		/// <returns>The statement kind identifier.</returns>
		public override string ToString()
		{
			return Kind;
		}

		/// <summary>
		/// Display name of the statement kind. Custom kinds are not translated, so the same
		/// <see cref="Kind"/> string is returned for every language.
		/// </summary>
		public ILocalizedString Name
		{ get { return _name ?? (_name = new LocalizedStringConstant(l => Kind)); } }

		private ILocalizedString _name;
	}
}
