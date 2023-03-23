using System;

namespace Inventor.Semantics.Serialization.Json.Answers
{
	[Serializable]
	public class BooleanAnswer : Answer
	{
		#region Properties

		public Boolean Result
		{ get; set; }

		#endregion

		#region Constructors

		public BooleanAnswer()
			: base()
		{ }

		public BooleanAnswer(Semantics.Answers.BooleanAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Result = answer.Result;
		}

		#endregion
	}
}