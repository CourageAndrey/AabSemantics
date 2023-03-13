using System.Collections.Generic;

using Inventor.Semantics.Mutations;

namespace Inventor.Semantics.Answers
{
	public class IsomorphicSearchAnswer : Answer, IAnswer<ICollection<KnowledgeStructure>>
	{
		#region Properties

		public ICollection<KnowledgeStructure> Result
		{ get; }

		#endregion

		public IsomorphicSearchAnswer(ICollection<KnowledgeStructure> result, IText description, IExplanation explanation)
			: base(description, explanation, result.Count == 0)
		{
			Result = result;
		}
	}
}
