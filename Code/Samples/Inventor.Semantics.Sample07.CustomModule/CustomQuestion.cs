using System;
using System.Collections.Generic;

using Inventor.Semantics;
using Inventor.Semantics.Questions;
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
			if (concept1 == null) throw new ArgumentNullException(nameof(concept1));
			if (concept2 == null) throw new ArgumentNullException(nameof(concept2));

			Concept1 = concept1;
			Concept2 = concept2;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<CustomQuestion, CustomStatement>()
				.Where(s => s.Concept1 == Concept1 && s.Concept2 == Concept2)
				.SelectBoolean(
					statements => statements.Count > 0,
					language => language.GetExtension<ILanguageCustomModule>().Questions.Answers.CustomTrue,
					language => language.GetExtension<ILanguageCustomModule>().Questions.Answers.CustomFalse,
					new Dictionary<String, IKnowledge>
					{
						{ CustomStatement.ParamConcept1, Concept1 },
						{ CustomStatement.ParamConcept2, Concept2 },
					});
		}
	}
}
