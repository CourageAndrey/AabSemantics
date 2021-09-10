using System;
using System.Collections.Generic;

using Inventor.Core.Attributes;
using Inventor.Core.Localization;
using Inventor.Core.Localization.Modules;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
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
						{ Strings.ParamConcept, Concept },
					});
		}
	}
}
