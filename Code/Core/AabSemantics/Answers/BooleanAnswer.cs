using System;

namespace AabSemantics.Answers
{
	/// <summary>Yes/no answer. Never empty: <c>false</c> means "no", not "unknown".</summary>
	public class BooleanAnswer : Answer, IAnswer<Boolean>
	{
		#region Properties

		/// <summary>Whether the asked condition holds.</summary>
		public Boolean Result
		{ get; }

		#endregion

		/// <summary>Creates a yes/no answer.</summary>
		/// <param name="result">Whether the asked condition holds.</param>
		/// <param name="description">The answer as localizable text.</param>
		/// <param name="explanation">Statements the answer was derived from.</param>
		public BooleanAnswer(Boolean result, IText description, IExplanation explanation)
			: base(description, explanation, false)
		{
			Result = result;
		}
	}
}
