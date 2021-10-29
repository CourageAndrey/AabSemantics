using System;

using Inventor.Semantics.WPF.TreeNodes;
using Inventor.Semantics.WPF.ViewModels.Statements;

namespace Inventor.Semantics.WPF.ViewModels
{
	public interface IViewModelFactory
	{
		IKnowledgeViewModel CreateByCoreType(Type type, ILanguage language);

		StatementViewModel CreateStatementByInstance(IStatement statement, ILanguage language);

		IKnowledgeViewModel CreateByTreeNode(ExtendedTreeNode treeNode, ILanguage language);
	}

	public class ViewModelFactory : IViewModelFactory
	{
		public virtual IKnowledgeViewModel CreateByCoreType(Type type, ILanguage language)
		{
			if (type == typeof(Semantics.Concepts.Concept))
			{
				return new Concept(language);
			}
			else if (type == typeof(Semantics.Set.Statements.HasPartStatement))
			{
				return new HasPartStatement(language);
			}
			else if (type == typeof(Semantics.Set.Statements.GroupStatement))
			{
				return new GroupStatement(language);
			}
			else if (type == typeof(Semantics.Set.Statements.HasSignStatement))
			{
				return new HasSignStatement(language);
			}
			else if (type == typeof(Semantics.Statements.IsStatement))
			{
				return new IsStatement(language);
			}
			else if (type == typeof(Semantics.Set.Statements.SignValueStatement))
			{
				return new SignValueStatement(language);
			}
			else if (type == typeof(Semantics.Mathematics.Statements.ComparisonStatement))
			{
				return new ComparisonStatement(language);
			}
			else if (type == typeof(Semantics.Processes.Statements.ProcessesStatement))
			{
				return new ProcessesStatement(language);
			}
			else
			{
				throw new NotSupportedException(type.FullName);
			}
		}

		public virtual StatementViewModel CreateStatementByInstance(IStatement statement, ILanguage language)
		{
			if (statement is Semantics.Set.Statements.HasPartStatement)
			{
				return new HasPartStatement(statement as Semantics.Set.Statements.HasPartStatement, language);
			}
			else if (statement is Semantics.Set.Statements.GroupStatement)
			{
				return new GroupStatement(statement as Semantics.Set.Statements.GroupStatement, language);
			}
			else if (statement is Semantics.Set.Statements.HasSignStatement)
			{
				return new HasSignStatement(statement as Semantics.Set.Statements.HasSignStatement, language);
			}
			else if (statement is Semantics.Statements.IsStatement)
			{
				return new IsStatement(statement as Semantics.Statements.IsStatement, language);
			}
			else if (statement is Semantics.Set.Statements.SignValueStatement)
			{
				return new SignValueStatement(statement as Semantics.Set.Statements.SignValueStatement, language);
			}
			else if (statement is Semantics.Mathematics.Statements.ComparisonStatement)
			{
				return new ComparisonStatement(statement as Semantics.Mathematics.Statements.ComparisonStatement, language);
			}
			else if (statement is Semantics.Processes.Statements.ProcessesStatement)
			{
				return new ProcessesStatement(statement as Semantics.Processes.Statements.ProcessesStatement, language);
			}
			else
			{
				throw new NotSupportedException(statement.GetType().FullName);
			}
		}

		public virtual IKnowledgeViewModel CreateByTreeNode(ExtendedTreeNode treeNode, ILanguage language)
		{
			var conceptNode = treeNode as ConceptNode;
			var statementNode = treeNode as StatementNode;

			if (conceptNode != null)
			{
				return new Concept(conceptNode.Concept as Semantics.Concepts.Concept);
			}
			else if (statementNode != null)
			{
				if (statementNode.Statement is Semantics.Set.Statements.HasPartStatement)
				{
					return new HasPartStatement(statementNode.Statement as Semantics.Set.Statements.HasPartStatement, language);
				}
				else if (statementNode.Statement is Semantics.Set.Statements.GroupStatement)
				{
					return new GroupStatement(statementNode.Statement as Semantics.Set.Statements.GroupStatement, language);
				}
				else if (statementNode.Statement is Semantics.Set.Statements.HasSignStatement)
				{
					return new HasSignStatement(statementNode.Statement as Semantics.Set.Statements.HasSignStatement, language);
				}
				else if (statementNode.Statement is Semantics.Statements.IsStatement)
				{
					return new IsStatement(statementNode.Statement as Semantics.Statements.IsStatement, language);
				}
				else if (statementNode.Statement is Semantics.Set.Statements.SignValueStatement)
				{
					return new SignValueStatement(statementNode.Statement as Semantics.Set.Statements.SignValueStatement, language);
				}
				else if (statementNode.Statement is Semantics.Mathematics.Statements.ComparisonStatement)
				{
					return new ComparisonStatement(statementNode.Statement as Semantics.Mathematics.Statements.ComparisonStatement, language);
				}
				else if (statementNode.Statement is Semantics.Processes.Statements.ProcessesStatement)
				{
					return new ProcessesStatement(statementNode.Statement as Semantics.Processes.Statements.ProcessesStatement, language);
				}
			}

			throw new NotSupportedException(treeNode.GetType().FullName);
		}
	}
}
