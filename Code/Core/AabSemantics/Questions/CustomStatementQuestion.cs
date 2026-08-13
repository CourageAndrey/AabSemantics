using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Statements;

namespace AabSemantics.Questions
{
	/// <summary>
	/// Looks up custom statements matching a kind and a set of roles. Both criteria are optional
	/// and act as filters, so an empty question returns every custom statement in the network.
	/// </summary>
	public class CustomStatementQuestion : Question
	{
		#region Properties

		/// <summary>Statement kind to match; <c>null</c> or empty matches any kind.</summary>
		public String Type
		{ get; set; }

		/// <summary>Role concepts a statement must have; a statement may carry further roles beyond these.</summary>
		public IDictionary<String, IConcept> Concepts
		{ get; set; }

		#endregion

		/// <summary>Creates the question.</summary>
		/// <param name="type">Statement kind to match; <c>null</c> matches any.</param>
		/// <param name="concepts">Role concepts to require; an empty map when <c>null</c>.</param>
		/// <param name="preconditions">Hypothetical statements to assume while answering.</param>
		public CustomStatementQuestion(String type = null, IDictionary<String, IConcept> concepts = null, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Type = type;
			Concepts = concepts ?? new Dictionary<String, IConcept>();
		}

		/// <summary>Selects the custom statements satisfying both filters.</summary>
		/// <param name="context">Context to search.</param>
		/// <returns>An answer listing the matching statements.</returns>
		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
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
				.SelectStatementsAsync();
		}
	}
}
