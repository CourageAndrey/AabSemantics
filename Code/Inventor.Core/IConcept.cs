using Inventor.Core.Localization;

namespace Inventor.Core
{
	public interface IConcept : INamed
	{
		LocalizedString Hint
		{ get; }

		ConceptType Type
		{ get; }
	}

	public enum ConceptType
	{
		System,
		Usual,
	}
}
