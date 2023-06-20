using System;
using System.Collections.Generic;

using Inventor.Semantics.Questions;
using Inventor.Semantics.Modules.Set.Attributes;
using Inventor.Semantics.Modules.Set.Localization;
using Inventor.Semantics.Modules.Set.Statements;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Modules.Set.Questions
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

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<IsSignQuestion, HasSignStatement>()
				.Where(s => s.Sign == Concept)
				.SelectBoolean(
					statement => Concept.HasAttribute<IsSignAttribute>(),
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.SignTrue,
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.SignFalse,
					new Dictionary<String, IKnowledge>
					{
						{ Semantics.Localization.Strings.ParamConcept, Concept },
					});
		}
	}
}
