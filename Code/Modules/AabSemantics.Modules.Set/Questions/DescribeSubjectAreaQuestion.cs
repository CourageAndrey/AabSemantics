using System.Collections.Generic;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
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
			Concept = concept.EnsureNotNull(nameof(concept));
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
