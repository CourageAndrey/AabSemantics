using System;
using System.Collections.Generic;

using Inventor.Core;
using Inventor.Core.Questions;
using Inventor.Set.Localization;
using Inventor.Set.Statements;

namespace Inventor.Set.Questions
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
					Core.Localization.Strings.ParamParent,
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.EnumerateParts);
		}
	}
}
