using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class EnumeratePartsQuestion : Question
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
				.From<EnumeratePartsQuestion, HasPartStatement>(DoesStatementMatch)
				.Select(CreateAnswer);
		}

		private IAnswer CreateAnswer(IQuestionProcessingContext<EnumeratePartsQuestion> context, ICollection<HasPartStatement> statements, ICollection<ChildAnswer> childAnswers)
		{
			if (statements.Any())
			{
				String format;
				var parameters = statements.Select(r => r.Part).ToList().Enumerate(out format);
				parameters.Add(Strings.ParamParent, Concept);
				return new ConceptsAnswer(
					statements.Select(s => s.Part).ToList(),
					new FormattedText(() => context.Language.Answers.EnumerateParts + format + ".", parameters),
					new Explanation(statements));
			}

			var childAnswer = childAnswers.FirstOrDefault();
			if (childAnswer != null)
			{
				childAnswer.Answer.Explanation.Expand(childAnswer.TransitiveStatements);
				return childAnswer.Answer;
			}

			return Answer.CreateUnknown(context.Language);
		}

		private Boolean DoesStatementMatch(HasPartStatement statement)
		{
			return statement.Whole == Concept;
		}
	}
}
