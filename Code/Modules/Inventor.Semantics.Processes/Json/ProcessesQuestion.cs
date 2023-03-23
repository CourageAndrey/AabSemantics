using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Processes.Json
{
	[DataContract]
	public class ProcessesQuestion : Serialization.Json.Question<Questions.ProcessesQuestion>
	{
		#region Properties

		[DataMember]
		public String ProcessA
		{ get; set; }

		[DataMember]
		public String ProcessB
		{ get; set; }

		#endregion

		#region Constructors

		public ProcessesQuestion()
			: base()
		{ }

		public ProcessesQuestion(Questions.ProcessesQuestion question)
			: base(question)
		{
			ProcessA = question.ProcessA.ID;
			ProcessB = question.ProcessB.ID;
		}

		#endregion

		protected override Questions.ProcessesQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.ProcessesQuestion(
				conceptIdResolver.GetConceptById(ProcessA),
				conceptIdResolver.GetConceptById(ProcessB),
				preconditions);
		}
	}
}
