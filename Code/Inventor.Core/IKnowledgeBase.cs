using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Utils;

namespace Inventor.Core
{
	public interface IKnowledgeBase : INamed, IChangeable
	{
		IKnowledgeBaseContext Context
		{ get; }

		ICollection<IConcept> Concepts
		{ get; }

		ICollection<IStatement> Statements
		{ get; }

		event EventHandler<ItemEventArgs<IConcept>> ConceptAdded;
		event EventHandler<ItemEventArgs<IConcept>> ConceptRemoved;
		event EventHandler<ItemEventArgs<IStatement>> StatementAdded;
		event EventHandler<ItemEventArgs<IStatement>> StatementRemoved;

		void Save(String fileName);
	}

	public static class KnowledgeBaseHelper
	{
		public static FormattedText DescribeRules(this IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var result = new FormattedText();
			foreach (var statement in knowledgeBase.Statements)
			{
				result.Add(statement.DescribeTrue(language));
			}
			return result;
		}

		public static FormattedText CheckConsistensy(this IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var result = new FormattedText();

			// 1. check all duplicates
			foreach (var statement in knowledgeBase.Statements)
			{
				if (!statement.CheckUnique(knowledgeBase.Statements))
				{
					result.Add(
						() => language.Misc.ConsistencyErrorDuplicate,
						new Dictionary<String, INamed> { { Strings.ParamStatement, statement } });
				}
			}

			// 2. check cyclic parents
			var clasifications = knowledgeBase.Statements.OfType<IsStatement>().ToList();
			foreach (var clasification in clasifications)
			{
				if (!clasification.CheckCyclic(clasifications))
				{
					result.Add(
						() => language.Misc.ConsistencyErrorCyclic,
						new Dictionary<String, INamed> { { Strings.ParamStatement, clasification } });
				}
			}

			// 4. check multi values
			var signValues = knowledgeBase.Statements.OfType<SignValueStatement>().ToList();
			foreach (var concept in knowledgeBase.Concepts)
			{
				var parents = clasifications.GetParentsOneLevel(concept);
				foreach (var sign in HasSignStatement.GetSigns(knowledgeBase.Statements, concept, true))
				{
					if (signValues.FirstOrDefault(sv => sv.Concept == concept && sv.Sign == sign.Sign) == null &&
						parents.Select(p => SignValueStatement.GetSignValue(knowledgeBase.Statements, p, sign.Sign)).Count(r => r != null) > 1)
					{
						result.Add(
							() => language.Misc.ConsistencyErrorMultipleSignValue,
							new Dictionary<String, INamed>
							{
								{ Strings.ParamConcept, concept },
								{ Strings.ParamSign, sign.Sign },
							});
					}
				}
			}

			// 5. check values without sign
			foreach (var signValue in signValues)
			{
				if (!signValue.CheckHasSign(knowledgeBase.Statements))
				{
					result.Add(
						() => language.Misc.ConsistencyErrorSignWithoutValue,
						new Dictionary<String, INamed> { { Strings.ParamStatement, signValue } });
				}
			}

			// 6. check sign duplications
			var hasSigns = knowledgeBase.Statements.OfType<HasSignStatement>().ToList();
			foreach (var hasSign in hasSigns)
			{
				if (!hasSign.CheckSignDuplication(hasSigns, clasifications))
				{
					result.Add(
						() => language.Misc.ConsistencyErrorMultipleSign,
						new Dictionary<String, INamed> { { Strings.ParamStatement, hasSign } });
				}
			}

			if (result.LinesCount == 0)
			{
				result.Add(() => language.Misc.CheckOk, new Dictionary<String, INamed>());
			}
			return result;
		}
	}
}
