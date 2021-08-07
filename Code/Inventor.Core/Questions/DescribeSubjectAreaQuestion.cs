using System;
using System.Collections.Generic;

using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
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
				.From<DescribeSubjectAreaQuestion, GroupStatement>(s => s.Area == Concept)
				.SelectConcepts(
					statement => statement.Concept,
					question => question.Concept,
					Strings.ParamArea,
					language => language.Answers.SubjectAreaConcepts);
		}
	}
}
