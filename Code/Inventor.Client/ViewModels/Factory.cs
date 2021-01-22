using System;

using Inventor.Client.TreeNodes;

namespace Inventor.Client.ViewModels
{
	internal static class Factory
	{
		public static IViewModel CreateByCoreType(Type type)
		{
			if (type == typeof(Core.Base.Concept))
			{
				return new Concept();
			}
			else if (type == typeof(Core.Statements.ConsistsOfStatement))
			{
				return new ConsistsOfStatement();
			}
			else if (type == typeof(Core.Statements.GroupStatement))
			{
				return new GroupStatement();
			}
			else if (type == typeof(Core.Statements.HasSignStatement))
			{
				return new HasSignStatement();
			}
			else if (type == typeof(Core.Statements.IsStatement))
			{
				return new IsStatement();
			}
			else if (type == typeof(Core.Statements.SignValueStatement))
			{
				return new SignValueStatement();
			}
			else
			{
				throw new NotSupportedException(type.FullName);
			}
		}

		public static StatementViewModel CreateStatementByInstance(Core.IStatement statement, Core.ILanguage language)
		{
			if (statement is Core.Statements.ConsistsOfStatement)
			{
				return new ConsistsOfStatement(statement as Core.Statements.ConsistsOfStatement, language);
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
			else
			{
				throw new NotSupportedException(statement.GetType().FullName);
			}
		}

		public static IViewModel CreateByTreeNode(ExtendedTreeNode treeNode, Core.ILanguage language)
		{
			var conceptNode = treeNode as ConceptNode;
			var statementNode = treeNode as StatementNode;

			if (conceptNode != null)
			{
				return new Concept(conceptNode.Concept as Core.Base.Concept);
			}
			else if (statementNode != null)
			{
				if (statementNode.Statement is Core.Statements.ConsistsOfStatement)
				{
					return new ConsistsOfStatement(statementNode.Statement as Core.Statements.ConsistsOfStatement, language);
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
			}

			throw new NotSupportedException(treeNode.GetType().FullName);
		}
	}
}
