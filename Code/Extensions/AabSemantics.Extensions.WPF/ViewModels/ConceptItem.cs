using System;

namespace AabSemantics.Extensions.WPF.ViewModels
{
	/// <summary>Entry of a concept pick list, pairing a concept with its rendered name.</summary>
	public class ConceptItem : IEquatable<ConceptItem>, INamed
	{
		/// <summary>The concept in question.</summary>
		public IConcept Concept
		{ get; }

		/// <summary>Display name.</summary>
		public ILocalizedString Name
		{ get { return Concept.Name; } }

		private readonly ILanguage _language;

		/// <summary>Creates the entry.</summary>
		/// <param name="concept">Concept the entry stands for.</param>
		/// <param name="language">Language its name is rendered in.</param>
		public ConceptItem(IConcept concept, ILanguage language)
		{
			Concept = concept;
			_language = language;
		}

		/// <summary>Formats the snapshot as its type name and message.</summary>
		/// <returns>Diagnostic string.</returns>
		public override string ToString()
		{
			return Concept.Name.GetValue(_language);
		}

		/// <summary>Compares entries by the concept they stand for.</summary>
		/// <param name="other">Entry to compare with; may be <c>null</c>.</param>
		/// <returns><c>true</c> when both stand for the same concept.</returns>
		public bool Equals(ConceptItem other)
		{
			return Concept == other?.Concept;
		}

		/// <summary>Returns a hash code consistent with <c>Equals</c>.</summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode()
		{
			return Concept.GetHashCode();
		}
	}
}
