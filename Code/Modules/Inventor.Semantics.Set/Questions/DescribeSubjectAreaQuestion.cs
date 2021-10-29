using System;
using System.Collections.Generic;

using Inventor.Semantics.Questions;
using Inventor.Semantics.Set.Localization;
using Inventor.Semantics.Set.Statements;

namespace Inventor.Semantics.Set.Questions
{
	public class DescribeSubjectAreaQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public DescribeSubjectAreaQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<DescribeSubjectAreaQuestion, GroupStatement>()
				.Where(s => s.Area == Concept)
				.SelectAllConcepts(
					statement => statement.Concept,
					question => question.Concept,
					Strings.ParamArea,
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.SubjectAreaConcepts);
		}
	}
}
