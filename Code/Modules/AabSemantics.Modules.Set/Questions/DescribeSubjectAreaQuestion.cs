using System.Collections.Generic;
using System.Threading.Tasks;

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

		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<DescribeSubjectAreaQuestion, GroupStatement>()
				.Where(s => s.Area == Concept)
				.SelectAllConceptsAsync(
					statement => statement.Concept,
					question => question.Concept,
					Strings.ParamArea,
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.SubjectAreaConcepts);
		}
	}
}
