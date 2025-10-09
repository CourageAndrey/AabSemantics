using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Localization;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Classification.Questions
{
	public class EnumerateAncestorsQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public EnumerateAncestorsQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
		}

		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<EnumerateAncestorsQuestion, IsStatement>()
				.Where(s => s.Descendant == Concept)
				.SelectAllConceptsAsync(
					statement => statement.Ancestor,
					question => question.Concept,
					Strings.ParamChild,
					language => language.GetQuestionsExtension<ILanguageClassificationModule, Localization.ILanguageQuestions>().Answers.EnumerateAncestors);
		}
	}
}
