using System;
using System.Collections.Generic;

using Inventor.Semantics;
using Inventor.Semantics.Questions;
using Inventor.Set.Localization;
using Inventor.Set.Statements;

namespace Inventor.Set.Questions
{
	public class EnumerateContainersQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public EnumerateContainersQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<EnumerateContainersQuestion, HasPartStatement>()
				.Where(s => s.Part == Concept)
				.SelectAllConcepts(
					statement => statement.Whole,
					question => question.Concept,
					Semantics.Localization.Strings.ParamChild,
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.EnumerateContainers);
		}
	}
}
