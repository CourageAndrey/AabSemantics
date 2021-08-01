using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Utils;

namespace Inventor.Core
{
	public interface ISemanticNetwork : INamed, IChangeable
	{
		ISemanticNetworkContext Context
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

	public static class SemanticNetworkHelper
	{
		public static FormattedText DescribeRules(this ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var result = new FormattedText();
			foreach (var statement in semanticNetwork.Statements)
			{
				result.Add(statement.DescribeTrue(language));
			}
			return result;
		}

		public static FormattedText CheckConsistensy(this ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var result = new FormattedText();

			// 1. check all duplicates
			foreach (var statement in semanticNetwork.Statements)
			{
				if (!statement.CheckUnique(semanticNetwork.Statements))
				{
					result.Add(
						() => language.Consistency.ErrorDuplicate,
						new Dictionary<String, INamed> { { Strings.ParamStatement, statement } });
				}
			}

			// 2. check cyclic parents
			var clasifications = semanticNetwork.Statements.OfType<IsStatement>().ToList();
			foreach (var clasification in clasifications)
			{
				if (!clasification.CheckCyclic(clasifications))
				{
					result.Add(
						() => language.Consistency.ErrorCyclic,
						new Dictionary<String, INamed> { { Strings.ParamStatement, clasification } });
				}
			}

			// 3. check multi values
			var signValues = semanticNetwork.Statements.OfType<SignValueStatement>().ToList();
			foreach (var concept in semanticNetwork.Concepts)
			{
				var parents = clasifications.GetParentsOneLevel(concept);
				foreach (var sign in HasSignStatement.GetSigns(semanticNetwork.Statements, concept, true))
				{
					if (signValues.FirstOrDefault(sv => sv.Concept == concept && sv.Sign == sign.Sign) == null &&
						parents.Select(p => SignValueStatement.GetSignValue(semanticNetwork.Statements, p, sign.Sign)).Count(r => r != null) > 1)
					{
						result.Add(
							() => language.Consistency.ErrorMultipleSignValue,
							new Dictionary<String, INamed>
							{
								{ Strings.ParamConcept, concept },
								{ Strings.ParamSign, sign.Sign },
							});
					}
				}
			}

			// 4. check values without sign
			foreach (var signValue in signValues)
			{
				if (!signValue.CheckHasSign(semanticNetwork.Statements))
				{
					result.Add(
						() => language.Consistency.ErrorSignWithoutValue,
						new Dictionary<String, INamed> { { Strings.ParamStatement, signValue } });
				}
			}

			// 5. check sign duplications
			var hasSigns = semanticNetwork.Statements.OfType<HasSignStatement>().ToList();
			foreach (var hasSign in hasSigns)
			{
				if (!hasSign.CheckSignDuplication(hasSigns, clasifications))
				{
					result.Add(
						() => language.Consistency.ErrorMultipleSign,
						new Dictionary<String, INamed> { { Strings.ParamStatement, hasSign } });
				}
			}

			// 6. Check comparison value systems
			foreach (var contradiction in semanticNetwork.Statements.OfType<ComparisonStatement>().CheckForContradictions())
			{
				String signsFormat;
				var concepts = contradiction.Signs.Enumerate(out signsFormat);
				concepts[Strings.ParamLeftValue] = contradiction.Value1;
				concepts[Strings.ParamRightValue] = contradiction.Value2;
				result.Add(
					() => language.Consistency.ErrorComparisonContradiction + signsFormat,
					concepts);
			}

			// 7. Check process sequence systems
			foreach (var contradiction in semanticNetwork.Statements.OfType<ProcessesStatement>().CheckForContradictions())
			{
				String signsFormat;
				var concepts = contradiction.Signs.Enumerate(out signsFormat);
				concepts[Strings.ParamProcessA] = contradiction.Value1;
				concepts[Strings.ParamProcessB] = contradiction.Value2;
				result.Add(
					() => language.Consistency.ErrorProcessesContradiction + signsFormat,
					concepts);
			}

			if (result.LinesCount == 0)
			{
				result.Add(() => language.Consistency.CheckOk, new Dictionary<String, INamed>());
			}
			return result;
		}
	}
}
