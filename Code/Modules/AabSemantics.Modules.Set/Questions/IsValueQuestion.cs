using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
{
	public class IsValueQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public IsValueQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
		}

		public override async Task<IAnswer> ProcessAsync(IQuestionProcessingContext context)
		{
			return await context
				.From<IsValueQuestion, SignValueStatement>()
				.Where(s => s.Value == Concept)
				.SelectBooleanAsync(
					statements => Concept.HasAttribute<IsValueAttribute>(),
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.ValueTrue,
					language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Answers.ValueFalse,
					new Dictionary<String, IKnowledge>
					{
						{ AabSemantics.Localization.Strings.ParamConcept, Concept },
					});
		}
	}
}
