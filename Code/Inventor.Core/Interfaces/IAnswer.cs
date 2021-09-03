using System;

namespace Inventor.Core
{
	public interface IAnswer
	{
		IText Description
		{ get; }

		IExplanation Explanation
		{ get; }

		Boolean IsEmpty
		{ get; }
	}

	public interface IAnswer<out TResult> : IAnswer
	{
		TResult Result
		{ get; }
	}
}
