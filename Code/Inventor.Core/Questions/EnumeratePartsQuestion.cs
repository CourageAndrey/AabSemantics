using System;
using System.Collections.Generic;

using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
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
					Strings.ParamParent,
					language => language.Answers.EnumerateParts)
				.IfEmptyTrySelectFirstChild()
				.Answer;
		}
	}
}
