using System;
using System.Collections.Generic;

using Inventor.Core.Localization;

namespace Inventor.Core.Base
{
	public class Concept : IConcept
	{
		#region Properties

		public ILocalizedString Name
		{ get; }

		public String ID
		{ get; private set; }

		public ILocalizedString Hint
		{ get; }

		public ICollection<IAttribute> Attributes
		{ get; }

		#endregion

		#region Constructors

		public Concept(String id = null, ILocalizedString name = null, ILocalizedString hint = null)
		{
			Name = name ?? new LocalizedStringVariable();
			ID = id.EnsureIdIsSet();
			Hint = hint ?? new LocalizedStringVariable();
			Attributes = new HashSet<IAttribute>();
		}

		#endregion

		public virtual void UpdateIdIfAllowed(String id)
		{
			ID = id.EnsureIdIsSet();
		}

		public override String ToString()
		{
			return this.GetTypeWithId();
		}
	}
}
