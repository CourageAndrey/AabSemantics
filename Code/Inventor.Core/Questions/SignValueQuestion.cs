using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class SignValueQuestion : Question<SignValueQuestion, SignValueStatement>
	{
		#region Properties

		public IConcept Concept
		{ get; }

		public IConcept Sign
		{ get; }

		#endregion

		public SignValueQuestion(IConcept concept, IConcept sign, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));
			if (sign == null) throw new ArgumentNullException(nameof(sign));

			Concept = concept;
			Sign = sign;
		}

		protected override IAnswer CreateAnswer(IQuestionProcessingContext<SignValueQuestion> context, ICollection<SignValueStatement> statements)
		{
			if (statements.Any())
			{
				var statement = statements.First();
				return new ConceptAnswer(
					statement.Value,
					formatSignValue(statement, context.Question.Concept, context.Language),
					new Explanation(statements));
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}

		protected override Boolean DoesStatementMatch(IQuestionProcessingContext<SignValueQuestion> context, SignValueStatement statement)
		{
			return statement.Concept == context.Question.Concept && statement.Sign == context.Question.Sign;
		}

		protected override IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<SignValueQuestion> context)
		{
			var alreadyViewedConcepts = new HashSet<IConcept>(context.ActiveContexts.OfType<IQuestionProcessingContext<SignValueQuestion>>().Select(questionContext => questionContext.Question.Concept));

			var question = context.Question;
			var transitiveStatements = context.KnowledgeBase.Statements.Enumerate<IsStatement>(context.ActiveContexts).Where(isStatement => isStatement.Child == question.Concept);

			foreach (var transitiveStatement in transitiveStatements)
			{
				var parent = transitiveStatement.Parent;
				if (!alreadyViewedConcepts.Contains(parent))
				{
					yield return new NestedQuestion(new SignValueQuestion(parent, question.Sign), new IStatement[] { transitiveStatement });
				}
			}
		}

		private static FormattedText formatSignValue(SignValueStatement value, IConcept original, ILanguage language)
		{
			return value != null
				? new FormattedText(
					() => language.Answers.SignValue,
					new Dictionary<String, INamed>
					{
						{ Strings.ParamConcept, original },
						{ Strings.ParamSign, value.Sign },
						{ Strings.ParamValue, value.Value },
						{ Strings.ParamDefined, value.Concept },
					})
				: null;
		}
	}
}
