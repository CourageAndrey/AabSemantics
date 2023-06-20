using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Modules.Set.Localization;
using Inventor.Semantics.Modules.Set.Statements;
using Inventor.Semantics.Questions;
using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Modules.Set.Questions
{
	[Obsolete("This class will be removed as soon as QuestionDialog supports CheckStatementQuestion. Please, use CheckStatementQuestion with corresponding statement instead.")]
	public class IsSubjectAreaQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public IConcept Area
		{ get; }

		#endregion

		public IsSubjectAreaQuestion(IConcept concept, IConcept area, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			Concept = concept.EnsureNotNull(nameof(concept));
			Area = area.EnsureNotNull(nameof(area));
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<IsSubjectAreaQuestion, GroupStatement>()
				.Where(s => s.Area == Area && s.Concept == Concept)
				.SelectBoolean(
					statements => statements.Any(),
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.IsSubjectAreaTrue,
					language => language.GetExtension<ILanguageSetModule>().Questions.Answers.IsSubjectAreaFalse,
					new Dictionary<String, IKnowledge>
					{
						{ Strings.ParamArea, Area },
						{ Semantics.Localization.Strings.ParamConcept, Concept },
					});
		}
	}
}
