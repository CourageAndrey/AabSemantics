namespace Inventor.Core
{
	public interface IConcept : IKnowledge
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
