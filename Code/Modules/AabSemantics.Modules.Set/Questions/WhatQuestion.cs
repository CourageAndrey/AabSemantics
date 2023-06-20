using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Answers;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Questions;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Questions
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
			Concept = concept.EnsureNotNull(nameof(concept));
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			var allStatements = context.SemanticNetwork.Statements.Enumerate(context.ActiveContexts).ToList();

			var isStatements = allStatements.Enumerate<IsStatement>(context.ActiveContexts).Where(c => c.Descendant == Concept).ToList();
			if (isStatements.Any())
			{
				var result = new Text.Containers.UnstructuredContainer();
				var explanation = new List<SignValueStatement>();
				foreach (var statement in isStatements)
				{
					var difference = getDifferenceBetweenAncestorAndDescendant(allStatements, statement);
					explanation.AddRange(difference);

					if (difference.Count > 0)
					{
						writeClassificationWithDifference(result, statement, difference);
					}
					else
					{
						writeJustClassification(result, statement);
					}

					result.AppendLineBreak();
				}
				return new Answer(result, new Explanation(explanation), false);
			}
			else
			{
				return Answer.CreateUnknown();
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

		private void writeClassificationWithDifference(ITextContainer result, IsStatement statement, List<SignValueStatement> difference)
		{
			result.Append(language => language.GetExtension<ILanguageSetModule>().Questions.Answers.IsDescriptionWithSign, new Dictionary<String, IKnowledge>
			{
				{ AabSemantics.Localization.Strings.ParamChild, Concept },
				{ AabSemantics.Localization.Strings.ParamParent, statement.Ancestor },
			});

			foreach (var diff in difference)
			{
				writeSignDifference(result, diff);
			}
		}

		private static void writeSignDifference(ITextContainer result, SignValueStatement diff)
		{
			result.Append(language => language.GetExtension<ILanguageSetModule>().Questions.Answers.IsDescriptionWithSignValue, new Dictionary<String, IKnowledge>
			{
				{ Strings.ParamSign, diff.Sign },
				{ Strings.ParamValue, diff.Value },
			});
		}

		private void writeJustClassification(ITextContainer result, IsStatement statement)
		{
			result.Append(language => language.GetExtension<ILanguageSetModule>().Questions.Answers.IsDescription, new Dictionary<String, IKnowledge>
			{
				{ AabSemantics.Localization.Strings.ParamChild, Concept },
				{ AabSemantics.Localization.Strings.ParamParent, statement.Ancestor },
			});
		}
	}
}
