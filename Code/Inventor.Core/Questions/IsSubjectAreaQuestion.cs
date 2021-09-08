using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
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
			if (concept == null) throw new ArgumentNullException(nameof(concept));
			if (area == null) throw new ArgumentNullException(nameof(area));

			Concept = concept;
			Area = area;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<IsSubjectAreaQuestion, GroupStatement>()
				.Where(s => s.Area == Area && s.Concept == Concept)
				.SelectBoolean(
					statements => statements.Any(),
					language => language.Questions.Answers.IsSubjectAreaTrue,
					language => language.Questions.Answers.IsSubjectAreaFalse,
					new Dictionary<String, IKnowledge>
					{
						{ Strings.ParamArea, Area },
						{ Strings.ParamConcept, Concept },
					});
		}
	}
}
