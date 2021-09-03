using System;

using Inventor.Core.Localization;

namespace Inventor.Core.Concepts
{
	public class SystemConcept : Concept
	{
		#region Constructors

		public SystemConcept(String id, LocalizedStringConstant name, LocalizedStringConstant hint)
			: base(id, name, hint)
		{ }

		#endregion

		public override void UpdateIdIfAllowed(String id)
		{
			if (id != ID)
			{
				throw new NotSupportedException();
			}
		}
	}
}
