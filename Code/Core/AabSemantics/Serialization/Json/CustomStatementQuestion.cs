using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AabSemantics.Serialization.Json
{
	[DataContract]
	public class CustomStatementQuestion : Question<Questions.CustomStatementQuestion>
	{
		#region Properties

		[DataMember]
		public String Type
		{ get; set; }

		[DataMember]
		public Dictionary<String, String> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		public CustomStatementQuestion()
			: base()
		{ }

		public CustomStatementQuestion(Questions.CustomStatementQuestion question)
			: base(question)
		{
			Type = question.Type;
			Concepts = question.Concepts.ToDictionary(
				c => c.Key,
				c => c.Value.ID);
		}

		#endregion

		protected override Questions.CustomStatementQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.CustomStatementQuestion(
				Type,
				Concepts.ToDictionary(
					c => c.Key,
					c => conceptIdResolver.GetConceptById(c.Value)),
				preconditions);
		}
	}
}
