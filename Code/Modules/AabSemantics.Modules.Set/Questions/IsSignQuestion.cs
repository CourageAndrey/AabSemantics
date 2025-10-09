using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
{
	public class IsSignQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public IsSignQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
		}

		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<IsSignQuestion, HasSignStatement>()
				.Where(s => s.Sign == Concept)
				.SelectBooleanAsync(
					statement => Concept.HasAttribute<IsSignAttribute>(),
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.SignTrue,
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.SignFalse,
					new Dictionary<String, IKnowledge>
					{
						{ AabSemantics.Localization.Strings.ParamConcept, Concept },
					});
		}
	}
}
