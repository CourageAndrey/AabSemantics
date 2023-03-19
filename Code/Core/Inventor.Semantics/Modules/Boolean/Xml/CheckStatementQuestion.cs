using System.Collections.Generic;
using System.Xml.Serialization;

using Inventor.Semantics.Metadata;
using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Xml;

namespace Inventor.Semantics.Modules.Boolean.Xml
{
	[XmlType]
	public class CheckStatementQuestion : Question<Questions.CheckStatementQuestion>
	{
		#region Properties

		[XmlElement]
		public Statement Statement
		{ get; set; }

		#endregion

		#region Constructors

		public CheckStatementQuestion()
		{ }

		public CheckStatementQuestion(Questions.CheckStatementQuestion question)
		{
			Statement = Repositories.Statements.Definitions[typeof(Questions.CheckStatementQuestion)].GetXml(question.Statement);
		}

		#endregion

		protected override Questions.CheckStatementQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.CheckStatementQuestion(
				Statement.Save(conceptIdResolver),
				preconditions);
		}
	}
}
