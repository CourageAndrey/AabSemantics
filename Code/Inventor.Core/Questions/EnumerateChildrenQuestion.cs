using System;
using System.Collections.Generic;

using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public class EnumerateChildrenQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public EnumerateChildrenQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<EnumerateChildrenQuestion, IsStatement>()
				.Where(s => s.Ancestor == Concept)
				.SelectAllConcepts(
					statement => statement.Descendant,
					question => question.Concept,
					Strings.ParamParent,
					language => language.Answers.Enumerate);
		}
	}
}
