using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Inventor.Semantics.Serialization.Json.Answers
{
	[DataContract]
	public class StatementsAnswer : Answer
	{
		#region Properties

		[DataMember]
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