namespace AabSemantics.Answers
{
	public class ConceptAnswer : Answer, IAnswer<IConcept>
	{
		#region Properties

		public IConcept Result
		{ get; }

		#endregion

		public ConceptAnswer(IConcept result, IText description, IExplanation explanation)
			: base(description, explanation, result == null)
		{
			Result = result;
		}
	}
}
