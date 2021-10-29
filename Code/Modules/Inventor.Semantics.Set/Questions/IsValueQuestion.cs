using System;
using System.Collections.Generic;

using Inventor.Semantics.Modules.Boolean.Attributes;
using Inventor.Semantics.Questions;
using Inventor.Semantics.Set.Localization;
using Inventor.Semantics.Set.Statements;

namespace Inventor.Semantics.Set.Questions
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
