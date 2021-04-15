using System;

namespace Inventor.Core
{
	public interface IAnswer
	{
		FormattedText Description
		{ get; }

		IExplanation Explanation
		{ get; }

		Boolean IsEmpty
		{ get; }
	}

	public interface IAnswer<TResult> : IAnswer
	{
		TResult Result
		{ get; }
	}
}
