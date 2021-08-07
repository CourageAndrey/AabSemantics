using System;
using System.Collections.Generic;

using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public class FindSubjectAreaQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public FindSubjectAreaQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<FindSubjectAreaQuestion, GroupStatement>(s => s.Concept == Concept)
				.SelectAllConcepts(
					statement => statement.Area,
					question => question.Concept,
					Strings.ParamConcept,
					language => language.Answers.SubjectArea)
				.IfEmptyTrySelectFirstChild()
				.Answer;
		}
	}
}
