using System;
using System.Collections.Generic;

using Inventor.Core.Localization;

namespace Inventor.Core.Base
{
	public sealed class Concept : IConcept
	{
		#region Properties

		public ILocalizedString Name
		{ get; }

		public ILocalizedString Hint
		{ get; }

		public ICollection<IAttribute> Attributes
		{ get; }

		#endregion

		#region Constructors

		public Concept(ILocalizedString name = null, ILocalizedString hint = null)
		{
			Name = name ?? new LocalizedStringVariable();
			Hint = hint ?? new LocalizedStringVariable();
			Attributes = new HashSet<IAttribute>();
		}

		#endregion

		public override String ToString()
		{
			return String.Format("{0} : {1}", Strings.TostringConcept, Name);
		}
	}
}
