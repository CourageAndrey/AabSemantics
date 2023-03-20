using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Inventor.Semantics.Localization;

namespace Inventor.Semantics.Serialization.Xml.Answers
{
	[XmlType]
	public class StatementsAnswer : Answer
	{
		#region Properties

		[XmlArray(nameof(Statements))]
		public List<Statement> Statements
		{ get; }

		#endregion

		#region Constructors

		public StatementsAnswer()
			: base(Semantics.Answers.Answer.CreateUnknown(), Language.Default)
		{
			Statements = new List<Statement>();
		}

		public StatementsAnswer(Semantics.Answers.StatementsAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Statements = answer.Result.Select(statement => Statement.Load(statement)).ToList();
		}

		#endregion
	}
}