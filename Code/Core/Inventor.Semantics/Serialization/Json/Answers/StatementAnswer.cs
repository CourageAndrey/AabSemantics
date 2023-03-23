using System;

namespace Inventor.Semantics.Serialization.Json.Answers
{
	[Serializable]
	public class StatementAnswer : Answer
	{
		#region Properties

		public Statement Statement
		{ get; set; }

		#endregion

		#region Constructors

		public StatementAnswer()
			: base()
		{ }

		public StatementAnswer(Semantics.Answers.StatementAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Statement = Statement.Load(answer.Result);
		}

		#endregion
	}
}