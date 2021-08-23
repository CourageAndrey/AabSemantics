namespace Inventor.Core
{
	public interface IKnowledge : INamed, IIdentifiable
	{
		ILocalizedString Hint
		{ get; }
	}
}
