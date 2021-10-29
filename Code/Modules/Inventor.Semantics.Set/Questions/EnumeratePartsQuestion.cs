using System;
using System.Collections.Generic;

using Inventor.Semantics.Questions;
using Inventor.Semantics.Set.Localization;
using Inventor.Semantics.Set.Statements;

namespace Inventor.Semantics.Set.Questions
{
	public class EnumeratePartsQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public EnumeratePartsQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<EnumeratePartsQuestion, HasPartStatement>()
				.Where(s => s.Whole == Concept)
				.SelectAllConcepts(
					statement => statement.Part,
					question => question.Concept,
					Semantics.Localization.Strings.ParamParent,
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.EnumerateParts);
		}
	}
}
