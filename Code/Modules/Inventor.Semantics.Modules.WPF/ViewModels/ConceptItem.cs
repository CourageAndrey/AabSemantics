using System;

namespace Inventor.Semantics.WPF.ViewModels
{
	public class ConceptItem : IEquatable<ConceptItem>, INamed
	{
		public IConcept Concept
		{ get; }

		public ILocalizedString Name
		{ get { return Concept.Name; } }

		private readonly ILanguage _language;

		public ConceptItem(IConcept concept, ILanguage language)
		{
			Concept = concept;
			_language = language;
		}

		public override string ToString()
		{
			return Concept.Name.GetValue(_language);
		}

		public bool Equals(ConceptItem other)
		{
			return Concept == other?.Concept;
		}
	}
}
