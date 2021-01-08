using Inventor.Core.Localization;

namespace Inventor.Core
{
	public sealed class Concept : INamed
	{
		#region Properties

		public LocalizedString Name
		{ get; }

		public LocalizedString Hint
		{ get; }

		internal ConceptType Type = ConceptType.Usual;

		#endregion

		#region Constructors

		public Concept(LocalizedString name = null, LocalizedString hint = null)
		{
			Name = name ?? new LocalizedStringVariable();
			Hint = hint ?? new LocalizedStringVariable();
		}

		#endregion

		public override string ToString()
		{
			return string.Format("{0} : {1}", Strings.TostringConcept, Name);
		}
	}

	internal enum ConceptType
	{
		System,
		Usual,
	}
}
