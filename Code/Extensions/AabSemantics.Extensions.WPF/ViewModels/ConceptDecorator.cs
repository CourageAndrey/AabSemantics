using System;
using System.Collections.Generic;

namespace AabSemantics.Extensions.WPF.ViewModels
{
	/// <summary>Wraps a concept so the UI can show it under a different rendered name.</summary>
	internal class ConceptDecorator : IConcept
	{
		#region Properties

		/// <summary>Display name.</summary>
		public ILocalizedString Name
		{ get { return Concept.Name; } }

		/// <summary>Identifier of the edited concept.</summary>
		public String ID
		{ get { return Concept.ID; } }

		public ILocalizedString Hint
		{ get { return Concept.Hint; } }

		public ICollection<IAttribute> Attributes
		{ get { return Concept.Attributes; } }

		/// <summary>The concept in question.</summary>
		public IConcept Concept
		{ get; }

		private readonly ILanguage _language;

		#endregion

		public ConceptDecorator(IConcept concept, ILanguage language)
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
	}
}
