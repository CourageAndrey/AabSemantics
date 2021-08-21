using System.Collections.Generic;

using Inventor.Core;

namespace Inventor.Client.ViewModels
{
	internal class ConceptDecorator : IConcept
	{
		#region Properties

		public ILocalizedString Name
		{ get { return Concept.Name; } }

		public ILocalizedString Hint
		{ get { return Concept.Hint; } }

		public ICollection<IAttribute> Attributes
		{ get { return Concept.Attributes; } }

		public IConcept Concept
		{ get; }

		private readonly ILanguage _language;

		#endregion

		public ConceptDecorator(IConcept concept, ILanguage language)
		{
			Concept = concept;
			_language = language;
		}

		public override string ToString()
		{
			return Concept.Name.GetValue(_language);
		}
	}
}
