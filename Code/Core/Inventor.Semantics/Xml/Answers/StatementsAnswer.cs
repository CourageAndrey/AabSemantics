using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Inventor.Semantics.Xml.Answers
{
	[XmlType]
	public class StatementsAnswer : Answer
	{
		#region Properties

		[XmlArray(nameof(Statements))]
		public ICollection<Statement> Statements
		{ get; }

		#endregion

		public StatementsAnswer(Semantics.Answers.StatementsAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Statements = answer.Result.Select(statement => Statement.Load(statement)).ToList();
		}
	}
}