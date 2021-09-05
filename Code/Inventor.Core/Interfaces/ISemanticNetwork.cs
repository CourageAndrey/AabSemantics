using System;
using System.Collections.Generic;

using Inventor.Core.Localization;
using Inventor.Core.Metadata;
using Inventor.Core.Text.Containers;
using Inventor.Core.Utils;

namespace Inventor.Core
{
	public interface ISemanticNetwork : INamed
	{
		ISemanticNetworkContext Context
		{ get; }

		ICollection<IConcept> Concepts
		{ get; }

		ICollection<IStatement> Statements
		{ get; }

		IDictionary<String, IExtensionModule> Modules
		{ get; }

		event EventHandler<ItemEventArgs<IConcept>> ConceptAdded;
		event EventHandler<ItemEventArgs<IConcept>> ConceptRemoved;
		event EventHandler<ItemEventArgs<IStatement>> StatementAdded;
		event EventHandler<ItemEventArgs<IStatement>> StatementRemoved;
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
				result.Append(language => language.Consistency.CheckOk);
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
						language => language.Consistency.ErrorDuplicate,
						new Dictionary<String, IKnowledge> { { Strings.ParamStatement, statement } });
				}
			}
		}
	}
}
