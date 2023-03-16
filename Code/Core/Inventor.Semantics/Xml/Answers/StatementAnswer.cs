using System.Xml.Serialization;

namespace Inventor.Semantics.Xml.Answers
{
	[XmlType]
	public class StatementAnswer : Answer
	{
		#region Properties

		[XmlElement]
		public Statement Statement
		{ get; }

		#endregion

		public StatementAnswer(Semantics.Answers.StatementAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Statement = Statement.Load(answer.Result);
		}
	}
}