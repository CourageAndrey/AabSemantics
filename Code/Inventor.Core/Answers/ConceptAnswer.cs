namespace Inventor.Core.Answers
{
	public class ConceptAnswer : Answer, IAnswer<IConcept>
	{
		#region Properties

		public IConcept Result
		{ get; }

		#endregion

		public ConceptAnswer(IConcept result, FormattedText description, IExplanation explanation)
			: base(description, explanation)
		{
			Result = result;
		}
	}
}
