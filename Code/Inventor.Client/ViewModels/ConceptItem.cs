using System;

using Inventor.Core;

namespace Inventor.Client.ViewModels
{
	public class ConceptItem : IEquatable<ConceptItem>
	{
		public IConcept Concept
		{ get; }

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
