using System;
using System.Xml.Serialization;

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

		public BooleanAnswer(Semantics.Answers.BooleanAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Result = answer.Result;
		}
	}
}