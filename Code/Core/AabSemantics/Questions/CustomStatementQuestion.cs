using System;
using System.Collections.Generic;

using AabSemantics.Statements;

namespace AabSemantics.Questions
{
	public class CustomStatementQuestion : Question
	{
		#region Properties

		public String Type
		{ get; set; }

		public IDictionary<String, IConcept> Concepts
		{ get; set; }

		#endregion

		public CustomStatementQuestion(String type = null, IDictionary<String, IConcept> concepts = null, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Type = type;
			Concepts = concepts ?? new Dictionary<String, IConcept>();
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<CustomStatementQuestion, CustomStatement>()
				.Where(statement =>
				{
					if (!string.IsNullOrEmpty(Type) && statement.Type != Type)
					{
						return false;
					}

					foreach (var concept in Concepts)
					{
						if (!statement.Concepts.TryGetValue(concept.Key, out var c) || c != concept.Value)
						{
							return false;
						}
					}

					return true;
				})
				.SelectStatements();
		}
	}
}
