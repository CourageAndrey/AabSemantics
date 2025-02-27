using System.Collections.Generic;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
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
					AabSemantics.Localization.Strings.ParamConcept,
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.SubjectArea);
		}
	}
}
