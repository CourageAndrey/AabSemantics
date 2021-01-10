using System;

using Inventor.Core.Localization;

namespace Inventor.Core
{
	public sealed class Concept : INamed
	{
		#region Properties

		public ILocalizedString Name
		{ get; }

		public LocalizedString Hint
		{ get; }

		public ConceptType Type
		{ get; internal set; } = ConceptType.Usual;

		#endregion

		#region Constructors

		public Concept(LocalizedString name = null, LocalizedString hint = null)
		{
			Name = name ?? new LocalizedStringVariable();
			Hint = hint ?? new LocalizedStringVariable();
		}

		#endregion

		public override String ToString()
		{
			return string.Format("{0} : {1}", Strings.TostringConcept, Name);
		}
	}

	public enum ConceptType
	{
		System,
		Usual,
	}
}
