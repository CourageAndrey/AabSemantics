using System;
using System.Collections.Generic;

using Inventor.Semantics;
using Inventor.Semantics.Questions;
using Inventor.Semantics.Text.Primitives;
using Inventor.Semantics.Utils;
using Samples.Semantics.Sample07.CustomModule.Localization;

namespace Samples.Semantics.Sample07.CustomModule
{
	public class CustomQuestion : Question
	{
		public IConcept Concept1
		{ get; }

		public IConcept Concept2
		{ get; }

		public CustomQuestion(IConcept concept1, IConcept concept2, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept1 = concept1.EnsureNotNull(nameof(concept1));
			Concept2 = concept2.EnsureNotNull(nameof(concept2));
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<CustomQuestion, CustomStatement>()
				.Where(s => s.Concept1 == Concept1 && s.Concept2 == Concept2)
				.SelectCustom((questionProcessingContext, statements, childAnswers) =>
				{
					bool isTrue = statements.Count > 0;
					var formatter = isTrue
						? new Func<ILanguage, String>(language => language.GetExtension<ILanguageCustomModule>().Questions.Answers.CustomTrue)
						: language => language.GetExtension<ILanguageCustomModule>().Questions.Answers.CustomFalse;
					var parameters = new Dictionary<String, IKnowledge>
					{
						{ CustomStatement.ParamConcept1, Concept1 },
						{ CustomStatement.ParamConcept2, Concept2 },
					};
					return new CustomAnswer(new FormattedText(formatter, parameters), new Explanation(statements));
				});
				//.SelectBoolean(
				//	statements => statements.Count > 0,
				//	language => language.GetExtension<ILanguageCustomModule>().Questions.Answers.CustomTrue,
				//	language => language.GetExtension<ILanguageCustomModule>().Questions.Answers.CustomFalse,
				//	new Dictionary<String, IKnowledge>
				//	{
				//		{ CustomStatement.ParamConcept1, Concept1 },
				//		{ CustomStatement.ParamConcept2, Concept2 },
				//	});
		}
	}
}
