using System;
using System.Xml.Serialization;

using Inventor.Semantics.Localization;

namespace Inventor.Semantics.Serialization.Xml.Answers
{
	[XmlType]
	public class BooleanAnswer : Answer
	{
		#region Properties

		[XmlElement]
		public Boolean Result
		{ get; }

		#endregion

		#region Constructors

		public BooleanAnswer()
			: base(Semantics.Answers.Answer.CreateUnknown(), Language.Default)
		{ }

		public BooleanAnswer(Semantics.Answers.BooleanAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Result = answer.Result;
		}

		#endregion
	}
}