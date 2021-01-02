using Inventor.Core.Localization;

namespace Inventor.Core
{
	public sealed class Concept : INamed
	{
		#region Properties

		public LocalizedString Name
		{ get { return name; } }

		public LocalizedString Hint
		{ get { return hint; } }

		internal ConceptType Type = ConceptType.Usual;

		private readonly LocalizedString name, hint;

		#endregion

		#region Constructors

		public Concept(LocalizedString name = null, LocalizedString hint = null)
		{
			this.name = name ?? new LocalizedStringVariable();
			this.hint = hint ?? new LocalizedStringVariable();
		}

		#endregion

		public override string ToString()
		{
			return string.Format("{0} : {1}", Strings.TostringConcept, Name.Value);
		}
	}

	internal enum ConceptType
	{
		System,
		Usual,
	}
}
