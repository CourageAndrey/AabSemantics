using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Answers;
using Inventor.Semantics.Mutations;
using Inventor.Semantics.Questions;
using Inventor.Semantics.Text.Containers;

namespace Inventor.Semantics.Questions
{
	public class IsomorphicSearchQuestion : Question
	{
		#region Properties

		public IsomorphicSearchPattern SearchPattern
		{ get; set; }

		public Boolean SelectMany
		{ get; set; }

		#endregion

		public IsomorphicSearchQuestion(IsomorphicSearchPattern searchPattern, Boolean selectMany = false, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (searchPattern == null) throw new ArgumentNullException(nameof(searchPattern));

			SearchPattern = searchPattern;
			SelectMany = false;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			ICollection<KnowledgeStructure> foundKnowledge;

			if (SelectMany)
			{
				foundKnowledge = SearchPattern.FindMatches(context.SemanticNetwork).ToList();
			}
			else
			{
				var match = context.SemanticNetwork.FindFirstMatch(SearchPattern);
				foundKnowledge = match != null
					? new[] { match }
					: Array.Empty<KnowledgeStructure>();
			}

			return new IsomorphicSearchAnswer(
				foundKnowledge,
				,
				);
		}
	}
}
