using System.Collections.Generic;

using AabSemantics.Localization;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Classification.Questions
{
	public class EnumerateDescendantsQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public EnumerateDescendantsQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<EnumerateDescendantsQuestion, IsStatement>()
				.Where(s => s.Ancestor == Concept)
				.SelectAllConcepts(
					statement => statement.Descendant,
					question => question.Concept,
					Strings.ParamParent,
					language => language.GetExtension<ILanguageClassificationModule>().Questions.Answers.EnumerateDescendants);
		}
	}
}
