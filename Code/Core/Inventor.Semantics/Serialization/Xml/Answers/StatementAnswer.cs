using System.Xml.Serialization;

using Inventor.Semantics.Localization;

namespace Inventor.Semantics.Serialization.Xml.Answers
{
	[XmlType]
	public class StatementAnswer : Answer
	{
		#region Properties

		[XmlElement]
		public Statement Statement
		{ get; }

		#endregion

		#region Constructors

		public StatementAnswer()
			: base(Semantics.Answers.Answer.CreateUnknown(), Language.Default)
		{ }

		public StatementAnswer(Semantics.Answers.StatementAnswer answer, ILanguage language)
			: base(answer, language)
		{
			Statement = Statement.Load(answer.Result);
		}

		#endregion
	}
}