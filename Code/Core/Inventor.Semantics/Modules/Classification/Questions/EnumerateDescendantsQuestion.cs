using System;
using System.Collections.Generic;

using Inventor.Semantics.Localization;
using Inventor.Semantics.Localization.Modules;
using Inventor.Semantics.Statements;

namespace Inventor.Semantics.Questions
{
	public class EnumerateDescendantsQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public EnumerateDescendantsQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<EnumerateDescendantsQuestion, IsStatement>()
				.Where(s => s.Ancestor == Concept)
				.SelectAllConcepts(
					statement => statement.Descendant,
					question => question.Concept,
					Strings.ParamParent,
					language => language.GetExtension<ILanguageClassificationModule>().Questions.Answers.EnumerateDescendants);
		}
	}
}
