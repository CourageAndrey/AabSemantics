using System.Collections.Generic;

namespace Inventor.Core.Answers
{
	public class ConceptsAnswer : Answer, IAnswer<ICollection<IConcept>>
	{
		#region Properties

		public ICollection<IConcept> Result
		{ get; }

		#endregion

		public ConceptsAnswer(ICollection<IConcept> result, FormattedText description, IExplanation explanation)
			: base(description, explanation)
		{
			Result = result;
		}
	}
}
