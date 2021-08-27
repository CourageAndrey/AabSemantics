using System.Collections.Generic;

namespace Inventor.Core.Answers
{
	public class ConceptsAnswer : Answer, IAnswer<ICollection<IConcept>>
	{
		#region Properties

		public ICollection<IConcept> Result
		{ get; }

		#endregion

		public ConceptsAnswer(ICollection<IConcept> result, IText description, IExplanation explanation)
			: base(description, explanation, result.Count == 0)
		{
			Result = result;
		}
	}
}
