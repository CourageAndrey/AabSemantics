using System;
using System.Collections.Generic;

using Inventor.Core;
using Inventor.Core.Questions;
using Inventor.Set.Localization;
using Inventor.Set.Statements;

namespace Inventor.Set.Questions
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
				.From<FindSubjectAreaQuestion, GroupStatement>()
				.Where(s => s.Concept == Concept)
				.SelectAllConcepts(
					statement => statement.Area,
					question => question.Concept,
					Core.Localization.Strings.ParamConcept,
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.SubjectArea);
		}
	}
}
