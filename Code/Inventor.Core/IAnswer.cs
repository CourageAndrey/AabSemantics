using System;

namespace Inventor.Core
{
	public interface IAnswer
	{
		Object Result
		{ get; }

		FormattedText Description
		{ get; }

		IExplanation Explanation
		{ get; }
	}
}
