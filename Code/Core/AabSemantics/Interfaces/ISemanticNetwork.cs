using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Text.Containers;
using AabSemantics.Utils;

namespace AabSemantics
{
	/// <summary>
	/// A knowledge base: concepts, the statements relating them, and the modules that define
	/// which kinds of statements and questions are available. This is the engine's entry point.
	/// </summary>
	public interface ISemanticNetwork : INamed
	{
		/// <summary>
		/// Context this network's knowledge lives in; questions are asked against it.
		/// </summary>
		ISemanticNetworkContext Context
		{ get; }

		/// <summary>
		/// Concepts known to the network. Removing a concept cascades to every statement
		/// that refers to it.
		/// </summary>
		IRepository<IConcept> Concepts
		{ get; }

		/// <summary>
		/// Statements known to the network. Statements added without a context are attached
		/// to <see cref="Context"/> automatically.
		/// </summary>
		IRepository<IStatement> Statements
		{ get; }

		/// <summary>
		/// Extension modules registered with the network, keyed by module name.
		/// A module must be registered before its statements or questions can be used.
		/// </summary>
		IDictionary<String, IExtensionModule> Modules
		{ get; }
	}

	/// <summary>
	/// Whole-network operations: rendering everything the network knows, and validating it.
	/// </summary>
	public static class SemanticNetworkHelper
	{
		/// <summary>
		/// Renders every statement in the network as an affirmative sentence.
		/// </summary>
		/// <param name="semanticNetwork">Network to describe.</param>
		/// <returns>Text containing one sentence per statement.</returns>
		public static async Task<IText> DescribeRulesAsync(this ISemanticNetwork semanticNetwork)
		{
			var result = new UnstructuredContainer();

			await Task.Run(() =>
			{
				foreach (var statement in semanticNetwork.Statements)
				{
					result.Append(statement.DescribeTrue());
				}
			}).ConfigureAwait(false);

			return result;
		}

		/// <summary>
		/// Validates the network: reports duplicated statements first, then runs the
		/// consistency check every registered statement type defines (contradictions, cycles
		/// in parent-child relations, and so on).
		/// </summary>
		/// <param name="semanticNetwork">Network to validate.</param>
		/// <returns>
		/// Text describing every problem found, or a single "check OK" line when the
		/// network is consistent.
		/// </returns>
		public static async Task<IText> CheckConsistencyAsync(this ISemanticNetwork semanticNetwork)
		{
			var result = new UnstructuredContainer();

			// 1. check all duplicates
			await CheckStatementDuplicatesAsync(semanticNetwork, result).ConfigureAwait(false);

			// 2. check specific statements
			foreach (var statementDefinition in Repositories.Statements.Definitions.Values)
			{
				await statementDefinition.CheckConsistencyAsync(semanticNetwork, result).ConfigureAwait(false);
			}

			if (result.Items.Count == 0)
			{
				result.Append(language => language.Statements.Consistency.CheckOk);
			}
			return result;
		}

		private static async Task CheckStatementDuplicatesAsync(ISemanticNetwork semanticNetwork, ITextContainer result)
		{
			foreach (var statement in semanticNetwork.Statements)
			{
				if (! await statement.CheckUniqueAsync(semanticNetwork.Statements).ConfigureAwait(false))
				{
					result.Append(
						language => language.Statements.Consistency.ErrorDuplicate,
						new Dictionary<String, IKnowledge> { { Strings.ParamStatement, statement } });
				}
			}
		}
		
		/// <summary>
		/// Blocking counterpart of <see cref="DescribeRulesAsync"/>, for callers that cannot await.
		/// </summary>
		/// <param name="semanticNetwork">Network to describe.</param>
		/// <returns>Text containing one sentence per statement.</returns>
		public static IText DescribeRules(this ISemanticNetwork semanticNetwork)
		{
			return TaskHelper.AwaitDetached(() => DescribeRulesAsync(semanticNetwork));
		}

		/// <summary>
		/// Blocking counterpart of <see cref="CheckConsistencyAsync"/>, for callers that cannot await.
		/// </summary>
		/// <param name="semanticNetwork">Network to validate.</param>
		/// <returns>Text describing every problem found, or a single "check OK" line.</returns>
		public static IText CheckConsistency(this ISemanticNetwork semanticNetwork)
		{
			return TaskHelper.AwaitDetached(() => CheckConsistencyAsync(semanticNetwork));
		}
	}
}
