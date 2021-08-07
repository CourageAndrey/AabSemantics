using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public class SignValueQuestion : Question
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

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return context
				.From<SignValueQuestion, SignValueStatement>(s => s.Concept == Concept && s.Sign == Sign)
				.WithTransitives(s => s.Count == 0, GetNestedQuestions)
				.SelectFirstConcept(
					statement => statement.Value,
					language => language.Answers.SignValue,
					statement => new Dictionary<String, INamed>
					{
						{ Strings.ParamConcept, Concept },
						{ Strings.ParamSign, statement.Sign },
						{ Strings.ParamValue, statement.Value },
						{ Strings.ParamDefined, statement.Concept },
					})
				.IfEmptyTrySelectFirstChild()
				.Answer;
		}

		private IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<SignValueQuestion> context)
		{
			var alreadyViewedConcepts = new HashSet<IConcept>(context.ActiveContexts.OfType<IQuestionProcessingContext<SignValueQuestion>>().Select(questionContext => questionContext.Question.Concept));

			var question = context.Question;
			var transitiveStatements = context.SemanticNetwork.Statements.Enumerate<IsStatement>(context.ActiveContexts).Where(isStatement => isStatement.Child == question.Concept);

			foreach (var transitiveStatement in transitiveStatements)
			{
				var parent = transitiveStatement.Parent;
				if (!alreadyViewedConcepts.Contains(parent))
				{
					yield return new NestedQuestion(new SignValueQuestion(parent, question.Sign), new IStatement[] { transitiveStatement });
				}
			}
		}
	}
}
