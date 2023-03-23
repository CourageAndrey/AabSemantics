using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Semantics.Serialization.Json.Answers
{
	[Serializable]
	public class StatementsAnswer : Answer
	{
		#region Properties

		public List<Statement> Statements
		{ get; set; }

		#endregion

		#region Constructors

		public StatementsAnswer()
			: base()
		{ }

		public StatementsAnswer(Semantics.Answers.StatementsAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Statements = answer.Result.Select(statement => Statement.Load(statement)).ToList();
		}

		#endregion
	}
}