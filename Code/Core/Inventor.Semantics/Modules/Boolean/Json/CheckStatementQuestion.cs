using System.Collections.Generic;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Json;

namespace Inventor.Semantics.Modules.Boolean.Json
{
	[DataContract]
	public class CheckStatementQuestion : Question<Questions.CheckStatementQuestion>
	{
		#region Properties

		[DataMember]
		public Statement Statement
		{ get; }

		#endregion

		#region Constructors

		public CheckStatementQuestion()
			: base()
		{ }

		public CheckStatementQuestion(Questions.CheckStatementQuestion question)
			: base(question)
		{
			Statement = Statement.Load(question.Statement);
		}

		#endregion

		protected override Questions.CheckStatementQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.CheckStatementQuestion(
				Statement.SaveOrReuse(conceptIdResolver, statementIdResolver),
				preconditions);
		}
	}
}
