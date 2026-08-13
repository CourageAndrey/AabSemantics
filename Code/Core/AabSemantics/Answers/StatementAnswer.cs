namespace AabSemantics.Answers
{
	/// <summary>Answer naming a single statement. Empty when no statement was found.</summary>
	public class StatementAnswer : Answer, IAnswer<IStatement>
	{
		#region Properties

		/// <summary>The statement found, or <c>null</c>.</summary>
		public IStatement Result
		{ get; }

		#endregion

		/// <summary>Creates a single-statement answer.</summary>
		/// <param name="result">The statement found; <c>null</c> makes the answer empty.</param>
		/// <param name="description">The answer as localizable text.</param>
		/// <param name="explanation">Statements the answer was derived from.</param>
		public StatementAnswer(IStatement result, IText description, IExplanation explanation)
			: base(description, explanation, result == null)
		{
			Result = result;
		}
	}
}
