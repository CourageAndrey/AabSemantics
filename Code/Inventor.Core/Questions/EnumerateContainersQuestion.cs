using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class EnumerateContainersQuestion : Question
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
				.Select(CreateAnswer);
		}

		private IAnswer CreateAnswer(IQuestionProcessingContext<EnumerateContainersQuestion> context, ICollection<HasPartStatement> statements, ICollection<ChildAnswer> childAnswers)
		{
			if (statements.Any())
			{
				String format;
				var parameters = statements.Select(r => r.Whole).ToList().Enumerate(out format);
				parameters.Add(Strings.ParamChild, Concept);
				return new ConceptsAnswer(
					statements.Select(s => s.Whole).ToList(),
					new FormattedText(() => context.Language.Answers.EnumerateContainers + format + ".", parameters),
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
	}
}
