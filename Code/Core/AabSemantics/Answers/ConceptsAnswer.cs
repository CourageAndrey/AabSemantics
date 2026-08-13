using System.Collections.Generic;

namespace AabSemantics.Answers
{
	/// <summary>Answer listing concepts. Empty when the list is empty.</summary>
	public class ConceptsAnswer : Answer, IAnswer<ICollection<IConcept>>
	{
		#region Properties

		/// <summary>The concepts found.</summary>
		public ICollection<IConcept> Result
		{ get; }

		#endregion

		/// <summary>Creates a concept-list answer.</summary>
		/// <param name="result">The concepts found; an empty collection makes the answer empty.</param>
		/// <param name="description">The answer as localizable text.</param>
		/// <param name="explanation">Statements the answer was derived from.</param>
		public ConceptsAnswer(ICollection<IConcept> result, IText description, IExplanation explanation)
			: base(description, explanation, result.Count == 0)
		{
			Result = result;
		}
	}
}
