using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Answers;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public class WhatQuestion : Question
	{
		#region Properties

		public IConcept Concept
		{ get; }

		#endregion

		public WhatQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			var allStatements = context.SemanticNetwork.Statements.Enumerate(context.ActiveContexts).ToList();

			var isStatements = allStatements.Enumerate<IsStatement>(context.ActiveContexts).Where(c => c.Descendant == Concept).ToList();
			if (isStatements.Any())
			{
				var result = new FormattedText();
				var explanation = new List<SignValueStatement>();
				foreach (var statement in isStatements)
				{
					var difference = getDifferenceBetweenAncestorAndDescendant(allStatements, statement);
					explanation.AddRange(difference);

					if (difference.Count > 0)
					{
						writeClassificationWithDifference(context.Language, result, statement, difference);
					}
					else
					{
						writeJustClassification(context.Language, result, statement);
					}

					result.AddEmptyLine();
				}
				return new Answer(result, new Explanation(explanation), false);
			}
			else
			{
				return Answer.CreateUnknown(context.Language);
			}
		}

		private List<SignValueStatement> getDifferenceBetweenAncestorAndDescendant(List<IStatement> allStatements, IsStatement isStatement)
		{
			var difference = new List<SignValueStatement>();
			foreach (var sign in HasSignStatement.GetSigns(allStatements, isStatement.Ancestor, false))
			{
				var signValue = SignValueStatement.GetSignValue(allStatements, Concept, sign.Sign);
				if (signValue != null)
				{
					difference.Add(signValue);
				}
			}
			return difference;
		}

		private void writeClassificationWithDifference(ILanguage language, FormattedText result, IsStatement statement, List<SignValueStatement> difference)
		{
			result.Add(() => language.Answers.IsDescriptionWithSign, new Dictionary<String, INamed>
			{
				{ Strings.ParamChild, Concept },
				{ Strings.ParamParent, statement.Ancestor },
			});

			foreach (var diff in difference)
			{
				writeSignDifference(language, result, diff);
			}
		}

		private static void writeSignDifference(ILanguage language, FormattedText result, SignValueStatement diff)
		{
			result.Add(() => language.Answers.IsDescriptionWithSignValue, new Dictionary<String, INamed>
			{
				{ Strings.ParamSign, diff.Sign },
				{ Strings.ParamValue, diff.Value },
			});
		}

		private void writeJustClassification(ILanguage language, FormattedText result, IsStatement statement)
		{
			result.Add(() => language.Answers.IsDescription, new Dictionary<String, INamed>
			{
				{ Strings.ParamChild, Concept },
				{ Strings.ParamParent, statement.Ancestor },
			});
		}
	}
}
