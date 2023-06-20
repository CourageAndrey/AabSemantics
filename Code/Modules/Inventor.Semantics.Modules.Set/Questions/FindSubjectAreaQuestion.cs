using System.Collections.Generic;

using Inventor.Semantics.Modules.Set.Localization;
using Inventor.Semantics.Modules.Set.Statements;
using Inventor.Semantics.Questions;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Modules.Set.Questions
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
			Concept = concept.EnsureNotNull(nameof(concept));
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<FindSubjectAreaQuestion, GroupStatement>()
				.Where(s => s.Concept == Concept)
				.SelectAllConcepts(
					statement => statement.Area,
					question => question.Concept,
					Semantics.Localization.Strings.ParamConcept,
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.SubjectArea);
		}
	}
}
