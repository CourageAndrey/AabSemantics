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
			else if (type == typeof(Core.Statements.AfterStatement))
			{
				return new AfterStatement(language);
			}
			else if (type == typeof(Core.Statements.BeforeStatement))
			{
				return new BeforeStatement(language);
			}
			else if (type == typeof(Core.Statements.CausesStatement))
			{
				return new CausesStatement(language);
			}
			else if (type == typeof(Core.Statements.MeanwhileStatement))
			{
				return new MeanwhileStatement(language);
			}
			else if (type == typeof(Core.Statements.IsEqualToStatement))
			{
				return new IsEqualToStatement(language);
			}
			else if (type == typeof(Core.Statements.IsGreaterThanOrEqualToStatement))
			{
				return new IsGreaterThanOrEqualToStatement(language);
			}
			else if (type == typeof(Core.Statements.IsGreaterThanStatement))
			{
				return new IsGreaterThanStatement(language);
			}
			else if (type == typeof(Core.Statements.IsLessThanOrEqualToStatement))
			{
				return new IsLessThanOrEqualToStatement(language);
			}
			else if (type == typeof(Core.Statements.IsLessThanStatement))
			{
				return new IsLessThanStatement(language);
			}
			else if (type == typeof(Core.Statements.IsNotEqualToStatement))
			{
				return new IsNotEqualToStatement(language);
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
			else if (statement is Core.Statements.AfterStatement)
			{
				return new AfterStatement(statement as Core.Statements.AfterStatement, language);
			}
			else if (statement is Core.Statements.BeforeStatement)
			{
				return new BeforeStatement(statement as Core.Statements.BeforeStatement, language);
			}
			else if (statement is Core.Statements.CausesStatement)
			{
				return new CausesStatement(statement as Core.Statements.CausesStatement, language);
			}
			else if (statement is Core.Statements.MeanwhileStatement)
			{
				return new MeanwhileStatement(statement as Core.Statements.MeanwhileStatement, language);
			}
			else if (statement is Core.Statements.IsEqualToStatement)
			{
				return new IsEqualToStatement(statement as Core.Statements.IsEqualToStatement, language);
			}
			else if (statement is Core.Statements.IsGreaterThanOrEqualToStatement)
			{
				return new IsGreaterThanOrEqualToStatement(statement as Core.Statements.IsGreaterThanOrEqualToStatement, language);
			}
			else if (statement is Core.Statements.IsGreaterThanStatement)
			{
				return new IsGreaterThanStatement(statement as Core.Statements.IsGreaterThanStatement, language);
			}
			else if (statement is Core.Statements.IsLessThanOrEqualToStatement)
			{
				return new IsLessThanOrEqualToStatement(statement as Core.Statements.IsLessThanOrEqualToStatement, language);
			}
			else if (statement is Core.Statements.IsLessThanStatement)
			{
				return new IsLessThanStatement(statement as Core.Statements.IsLessThanStatement, language);
			}
			else if (statement is Core.Statements.IsNotEqualToStatement)
			{
				return new IsNotEqualToStatement(statement as Core.Statements.IsNotEqualToStatement, language);
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
				else if (statementNode.Statement is Core.Statements.AfterStatement)
				{
					return new AfterStatement(statementNode.Statement as Core.Statements.AfterStatement, language);
				}
				else if (statementNode.Statement is Core.Statements.BeforeStatement)
				{
					return new BeforeStatement(statementNode.Statement as Core.Statements.BeforeStatement, language);
				}
				else if (statementNode.Statement is Core.Statements.CausesStatement)
				{
					return new CausesStatement(statementNode.Statement as Core.Statements.CausesStatement, language);
				}
				else if (statementNode.Statement is Core.Statements.MeanwhileStatement)
				{
					return new MeanwhileStatement(statementNode.Statement as Core.Statements.MeanwhileStatement, language);
				}
				else if (statementNode.Statement is Core.Statements.IsEqualToStatement)
				{
					return new IsEqualToStatement(statementNode.Statement as Core.Statements.IsEqualToStatement, language);
				}
				else if (statementNode.Statement is Core.Statements.IsGreaterThanOrEqualToStatement)
				{
					return new IsGreaterThanOrEqualToStatement(statementNode.Statement as Core.Statements.IsGreaterThanOrEqualToStatement, language);
				}
				else if (statementNode.Statement is Core.Statements.IsGreaterThanStatement)
				{
					return new IsGreaterThanStatement(statementNode.Statement as Core.Statements.IsGreaterThanStatement, language);
				}
				else if (statementNode.Statement is Core.Statements.IsLessThanOrEqualToStatement)
				{
					return new IsLessThanOrEqualToStatement(statementNode.Statement as Core.Statements.IsLessThanOrEqualToStatement, language);
				}
				else if (statementNode.Statement is Core.Statements.IsLessThanStatement)
				{
					return new IsLessThanStatement(statementNode.Statement as Core.Statements.IsLessThanStatement, language);
				}
				else if (statementNode.Statement is Core.Statements.IsNotEqualToStatement)
				{
					return new IsNotEqualToStatement(statementNode.Statement as Core.Statements.IsNotEqualToStatement, language);
				}
			}

			throw new NotSupportedException(treeNode.GetType().FullName);
		}
	}
}
