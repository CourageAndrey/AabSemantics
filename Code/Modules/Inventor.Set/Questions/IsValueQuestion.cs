using System;
using System.Collections.Generic;

using Inventor.Semantics;
using Inventor.Semantics.Attributes;
using Inventor.Semantics.Questions;
using Inventor.Set.Localization;
using Inventor.Set.Statements;

namespace Inventor.Set.Questions
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
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<IsValueQuestion, SignValueStatement>()
				.Where(s => s.Value == Concept)
				.SelectBoolean(
					statements => Concept.HasAttribute<IsValueAttribute>(),
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.ValueTrue,
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.ValueFalse,
					new Dictionary<String, IKnowledge>
					{
						{ Semantics.Localization.Strings.ParamConcept, Concept },
					});
		}
	}
}
