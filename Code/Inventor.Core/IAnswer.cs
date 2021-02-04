namespace Inventor.Core
{
	public interface IAnswer
	{
		FormattedText Description
		{ get; }

		IExplanation Explanation
		{ get; }
	}

	public interface IAnswer<TResult> : IAnswer
	{
		TResult Result
		{ get; }
	}
}
