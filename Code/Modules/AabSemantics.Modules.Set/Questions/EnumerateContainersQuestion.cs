using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
{
	public class EnumerateContainersQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public EnumerateContainersQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
		}

		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<EnumerateContainersQuestion, HasPartStatement>()
				.Where(s => s.Part == Concept)
				.SelectAllConceptsAsync(
					statement => statement.Whole,
					question => question.Concept,
					AabSemantics.Localization.Strings.ParamChild,
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.EnumerateContainers);
		}
	}
}
