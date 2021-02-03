using System;

using Inventor.Client.TreeNodes;
using Inventor.Client.ViewModels.Statements;

namespace Inventor.Client.ViewModels
{
	internal static class Factory
	{
		public static IKnowledgeViewModel CreateByCoreType(Type type, Core.ILanguage language)
		{
			if (type == typeof(Core.Base.Concept))
			{
				return new Concept();
			}
			else if (type == typeof(Core.Statements.HasPartStatement))
			{
				return new HasPartStatement(language);
			}
			else if (type == typeof(Core.Statements.GroupStatement))
			{
				return new GroupStatement(language);
			}
			else if (type == typeof(Core.Statements.HasSignStatement))
			{
				return new HasSignStatement(language);
			}
			else if (type == typeof(Core.Statements.IsStatement))
			{
				return new IsStatement(language);
			}
			else if (type == typeof(Core.Statements.SignValueStatement))
			{
				return new SignValueStatement(language);
			}
			else if (type == typeof(Core.Statements.ComparisonStatement))
			{
				return new ComparisonStatement(language);
			}
			else if (type == typeof(Core.Statements.ProcessesStatement))
			{
				return new ProcessesStatement(language);
			}
			else
			{
				throw new NotSupportedException(type.FullName);
			}
		}

		public static StatementViewModel CreateStatementByInstance(Core.IStatement statement, Core.ILanguage language)
		{
			if (statement is Core.Statements.HasPartStatement)
			{
				return new HasPartStatement(statement as Core.Statements.HasPartStatement, language);
			}
			else if (statement is Core.Statements.GroupStatement)
			{
				return new GroupStatement(statement as Core.Statements.GroupStatement, language);
			}
			else if (statement is Core.Statements.HasSignStatement)
			{
				return new HasSignStatement(statement as Core.Statements.HasSignStatement, language);
			}
			else if (statement is Core.Statements.IsStatement)
			{
				return new IsStatement(statement as Core.Statements.IsStatement, language);
			}
			else if (statement is Core.Statements.SignValueStatement)
			{
				return new SignValueStatement(statement as Core.Statements.SignValueStatement, language);
			}
			else if (statement is Core.Statements.ComparisonStatement)
			{
				return new ComparisonStatement(statement as Core.Statements.ComparisonStatement, language);
			}
			else if (statement is Core.Statements.ProcessesStatement)
			{
				return new ProcessesStatement(statement as Core.Statements.ProcessesStatement, language);
			}
			else
			{
				throw new NotSupportedException(statement.GetType().FullName);
			}
		}

		public static IKnowledgeViewModel CreateByTreeNode(ExtendedTreeNode treeNode, Core.ILanguage language)
		{
			var conceptNode = treeNode as ConceptNode;
			var statementNode = treeNode as StatementNode;

			if (conceptNode != null)
			{
				return new Concept(conceptNode.Concept as Core.Base.Concept);
			}
			else if (statementNode != null)
			{
				if (statementNode.Statement is Core.Statements.HasPartStatement)
				{
					return new HasPartStatement(statementNode.Statement as Core.Statements.HasPartStatement, language);
				}
				else if (statementNode.Statement is Core.Statements.GroupStatement)
				{
					return new GroupStatement(statementNode.Statement as Core.Statements.GroupStatement, language);
				}
				else if (statementNode.Statement is Core.Statements.HasSignStatement)
				{
					return new HasSignStatement(statementNode.Statement as Core.Statements.HasSignStatement, language);
				}
				else if (statementNode.Statement is Core.Statements.IsStatement)
				{
					return new IsStatement(statementNode.Statement as Core.Statements.IsStatement, language);
				}
				else if (statementNode.Statement is Core.Statements.SignValueStatement)
				{
					return new SignValueStatement(statementNode.Statement as Core.Statements.SignValueStatement, language);
				}
				else if (statementNode.Statement is Core.Statements.ComparisonStatement)
				{
					return new ComparisonStatement(statementNode.Statement as Core.Statements.ComparisonStatement, language);
				}
				else if (statementNode.Statement is Core.Statements.ProcessesStatement)
				{
					return new ProcessesStatement(statementNode.Statement as Core.Statements.ProcessesStatement, language);
				}
			}

			throw new NotSupportedException(treeNode.GetType().FullName);
		}
	}
}
