using System;
using System.Collections.Generic;

using Inventor.Core.Answers;
using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public class IsSignQuestion : Question<IsSignQuestion, HasSignStatement>
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public IsSignQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		protected override IAnswer CreateAnswer(IQuestionProcessingContext<IsSignQuestion> context, ICollection<HasSignStatement> statements)
		{
			bool isSign = Concept.HasAttribute<IsSignAttribute>();
			return new BooleanAnswer(
				isSign,
				new FormattedText(
					isSign ? new Func<String>(() => context.Language.Answers.SignTrue) : () => context.Language.Answers.SignFalse,
					new Dictionary<String, INamed>
					{
						{ Strings.ParamConcept, Concept },
					}),
				new Explanation(statements));
		}

		protected override Boolean DoesStatementMatch(IQuestionProcessingContext<IsSignQuestion> context, HasSignStatement statement)
		{
			return statement.Sign == Concept;
		}

		protected override Boolean NeedToCheckTransitives(IQuestionProcessingContext<IsSignQuestion> context, ICollection<HasSignStatement> statements)
		{
			return false;
		}
	}
}
