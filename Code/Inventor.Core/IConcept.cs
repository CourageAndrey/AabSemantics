namespace Inventor.Core
{
	public interface IConcept : INamed
	{
		ILocalizedString Hint
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
