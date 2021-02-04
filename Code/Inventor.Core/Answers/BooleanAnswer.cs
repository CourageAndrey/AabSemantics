using System;

namespace Inventor.Core.Answers
{
	public class BooleanAnswer : Answer, IAnswer<Boolean>
	{
		#region Properties

		public Boolean Result
		{ get; }

		#endregion

		public BooleanAnswer(Boolean result, FormattedText description, IExplanation explanation)
			: base(description, explanation)
		{
			Result = result;
		}
	}
}
