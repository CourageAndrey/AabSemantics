using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public class IsSignQuestion : Question
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

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<IsSignQuestion, HasSignStatement>(DoesStatementMatch)
				.Select(CreateAnswer);
		}

		private IAnswer CreateAnswer(IQuestionProcessingContext<IsSignQuestion> context, ICollection<HasSignStatement> statements, ICollection<ChildAnswer> childAnswers)
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

		private Boolean DoesStatementMatch(HasSignStatement statement)
		{
			return statement.Sign == Concept;
		}
	}
}
