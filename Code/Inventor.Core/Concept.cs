using Inventor.Core.Localization;

namespace Inventor.Core
{
	public sealed class Concept : INamed
	{
		#region Properties

		public LocalizedString Name
		{ get { return _name; } }

		public LocalizedString Hint
		{ get { return _hint; } }

		internal ConceptType Type = ConceptType.Usual;

		private readonly LocalizedString _name;
		private readonly LocalizedString _hint;

		#endregion

		#region Constructors

		public Concept(LocalizedString name = null, LocalizedString hint = null)
		{
			_name = name ?? new LocalizedStringVariable();
			_hint = hint ?? new LocalizedStringVariable();
		}

		#endregion

		public override string ToString()
		{
			return string.Format("{0} : {1}", Strings.TostringConcept, Name.GetValue(LanguageEx.CurrentEx));
		}
	}

	internal enum ConceptType
	{
		System,
		Usual,
	}
}
