using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Text.Containers;
using AabSemantics.Utils;

namespace AabSemantics
{
	public interface ISemanticNetwork : INamed
	{
		ISemanticNetworkContext Context
		{ get; }

		IRepository<IConcept> Concepts
		{ get; }

		IRepository<IStatement> Statements
		{ get; }

		IDictionary<String, IExtensionModule> Modules
		{ get; }
	}

	public static class SemanticNetworkHelper
	{
		public static async Task<IText> DescribeRulesAsync(this ISemanticNetwork semanticNetwork)
		{
			var result = new UnstructuredContainer();

			await Task.Run(() =>
			{
				foreach (var statement in semanticNetwork.Statements)
				{
					result.Append(statement.DescribeTrue());
				}
			});

			return result;
		}

		public static async Task<IText> CheckConsistencyAsync(this ISemanticNetwork semanticNetwork)
		{
			var result = new UnstructuredContainer();

			// 1. check all duplicates
			await CheckStatementDuplicatesAsync(semanticNetwork, result);

			// 2. check specific statements
			foreach (var statementDefinition in Repositories.Statements.Definitions.Values)
			{
				await statementDefinition.CheckConsistencyAsync(semanticNetwork, result);
			}

			if (result.Items.Count == 0)
			{
				result.Append(language => language.Statements.Consistency.CheckOk);
			}
			return await Task.FromResult(result);
		}

		private static async Task CheckStatementDuplicatesAsync(ISemanticNetwork semanticNetwork, ITextContainer result)
		{
			foreach (var statement in semanticNetwork.Statements)
			{
				if (! await statement.CheckUniqueAsync(semanticNetwork.Statements))
				{
					result.Append(
						language => language.Statements.Consistency.ErrorDuplicate,
						new Dictionary<String, IKnowledge> { { Strings.ParamStatement, statement } });
				}
			}
		}
		
		public static IText DescribeRules(this ISemanticNetwork semanticNetwork)
		{
			return DescribeRulesAsync(semanticNetwork).Await();
		}

		public static IText CheckConsistency(this ISemanticNetwork semanticNetwork)
		{
			return CheckConsistencyAsync(semanticNetwork).Await();
		}
	}
}
