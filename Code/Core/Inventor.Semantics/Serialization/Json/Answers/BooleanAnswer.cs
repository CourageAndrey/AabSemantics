using System;
using System.Runtime.Serialization;

namespace Inventor.Semantics.Serialization.Json.Answers
{
	[DataContract]
	public class BooleanAnswer : Answer
	{
		#region Properties

		[DataMember]
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