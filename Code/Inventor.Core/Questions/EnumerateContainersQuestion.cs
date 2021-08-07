using System;
using System.Collections.Generic;

using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
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
				.From<EnumerateContainersQuestion, HasPartStatement>(s => s.Part == Concept)
				.SelectAllConcepts(
					statement => statement.Whole,
					question => question.Concept,
					Strings.ParamChild,
					language => language.Answers.EnumerateContainers)
				.IfEmptyTrySelectFirstChild()
				.Answer;
		}
	}
}
