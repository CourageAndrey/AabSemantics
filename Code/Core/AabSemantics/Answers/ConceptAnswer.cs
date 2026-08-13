namespace AabSemantics.Answers
{
	/// <summary>Answer naming a single concept. Empty when no concept was found.</summary>
	public class ConceptAnswer : Answer, IAnswer<IConcept>
	{
		#region Properties

		/// <summary>The concept found, or <c>null</c>.</summary>
		public IConcept Result
		{ get; }

		#endregion

		/// <summary>Creates a single-concept answer.</summary>
		/// <param name="result">The concept found; <c>null</c> makes the answer empty.</param>
		/// <param name="description">The answer as localizable text.</param>
		/// <param name="explanation">Statements the answer was derived from.</param>
		public ConceptAnswer(IConcept result, IText description, IExplanation explanation)
			: base(description, explanation, result == null)
		{
			Result = result;
		}
	}
}
