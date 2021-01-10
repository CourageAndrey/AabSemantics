using System.Collections.Generic;

using Inventor.Core.Localization;

namespace Inventor.Core
{
	public interface IStatement : INamed
	{
		LocalizedString Hint
		{ get; }

		IEnumerable<Concept> GetChildConcepts();
	}
}
