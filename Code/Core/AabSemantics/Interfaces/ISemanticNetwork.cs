using System;
using System.Collections.Generic;

using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Text.Containers;

namespace AabSemantics
{
	public interface ISemanticNetwork : INamed
	{
		ISemanticNetworkContext Context
		{ get; }

		IKeyedCollection<IConcept> Concepts
		{ get; }

		IKeyedCollection<IStatement> Statements
		{ get; }

		IDictionary<String, IExtensionModule> Modules
		{ get; }
	}

	public static class SemanticNetworkHelper
	{
		public static IText DescribeRules(this ISemanticNetwork semanticNetwork)
		{
			var result = new UnstructuredContainer();
			foreach (var statement in semanticNetwork.Statements)
			{
				result.Append(statement.DescribeTrue());
			}
			return result;
		}

		public static IText CheckConsistency(this ISemanticNetwork semanticNetwork)
		{
			var result = new UnstructuredContainer();

			// 1. check all duplicates
			checkStatementDuplicates(semanticNetwork, result);

			// 2. check specific statements
			foreach (var statementDefinition in Repositories.Statements.Definitions.Values)
			{
				statementDefinition.CheckConsistency(semanticNetwork, result);
			}

			if (result.Items.Count == 0)
			{
				result.Append(language => language.Statements.Consistency.CheckOk);
			}
			return result;
		}

		private static void checkStatementDuplicates(ISemanticNetwork semanticNetwork, ITextContainer result)
		{
			foreach (var statement in semanticNetwork.Statements)
			{
				if (!statement.CheckUnique(semanticNetwork.Statements))
				{
					result.Append(
						language => language.Statements.Consistency.ErrorDuplicate,
						new Dictionary<String, IKnowledge> { { Strings.ParamStatement, statement } });
				}
			}
		}
	}
}
