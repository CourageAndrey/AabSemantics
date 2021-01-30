using System;

using Inventor.Core.Localization;

namespace Inventor.Core.Base
{
	public sealed class Concept : IConcept
	{
		#region Properties

		public ILocalizedString Name
		{ get; }

		public IContext Context
		{ get; set; }

		public ILocalizedString Hint
		{ get; }

		#endregion

		#region Constructors

		public Concept(ILocalizedString name = null, ILocalizedString hint = null)
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
}
