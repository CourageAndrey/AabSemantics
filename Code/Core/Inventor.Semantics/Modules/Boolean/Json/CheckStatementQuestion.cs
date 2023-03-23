using System;
using System.Collections.Generic;

using Inventor.Semantics.Serialization;
using Inventor.Semantics.Serialization.Json;

namespace Inventor.Semantics.Modules.Boolean.Json
{
	[Serializable]
	public class CheckStatementQuestion : Question<Questions.CheckStatementQuestion>
	{
		#region Properties

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

		protected override Questions.CheckStatementQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.CheckStatementQuestion(
				Statement.Save(conceptIdResolver),
				preconditions);
		}
	}
}
