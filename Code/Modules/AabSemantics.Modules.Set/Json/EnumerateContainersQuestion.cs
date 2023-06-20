using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using AabSemantics.Serialization;

namespace AabSemantics.Modules.Set.Json
{
	[DataContract]
	public class EnumerateContainersQuestion : Serialization.Json.Question<Questions.EnumerateContainersQuestion>
	{
		#region Properties

		[DataMember]
		public String Concept
		{ get; set; }

		#endregion

		#region Constructors

		public EnumerateContainersQuestion()
			: base()
		{ }

		public EnumerateContainersQuestion(Questions.EnumerateContainersQuestion question)
			: base(question)
		{
			Concept = question.Concept.ID;
		}

		#endregion

		protected override Questions.EnumerateContainersQuestion SaveImplementation(ConceptIdResolver conceptIdResolver, StatementIdResolver statementIdResolver, IEnumerable<IStatement> preconditions)
		{
			return new Questions.EnumerateContainersQuestion(
				conceptIdResolver.GetConceptById(Concept),
				preconditions);
		}
	}
}
