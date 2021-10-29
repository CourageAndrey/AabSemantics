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
			if (type == typeof(Concepts.Concept))
			{
				return new Concept(language);
			}
			else if (type == typeof(Set.Statements.HasPartStatement))
			{
				return new HasPartStatement(language);
			}
			else if (type == typeof(Set.Statements.GroupStatement))
			{
				return new GroupStatement(language);
			}
			else if (type == typeof(Set.Statements.HasSignStatement))
			{
				return new HasSignStatement(language);
			}
			else if (type == typeof(Modules.Classification.Statements.IsStatement))
			{
				return new IsStatement(language);
			}
			else if (type == typeof(Set.Statements.SignValueStatement))
			{
				return new SignValueStatement(language);
			}
			else if (type == typeof(Mathematics.Statements.ComparisonStatement))
			{
				return new ComparisonStatement(language);
			}
			else if (type == typeof(Processes.Statements.ProcessesStatement))
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
			if (statement is Set.Statements.HasPartStatement)
			{
				return new HasPartStatement(statement as Set.Statements.HasPartStatement, language);
			}
			else if (statement is Set.Statements.GroupStatement)
			{
				return new GroupStatement(statement as Set.Statements.GroupStatement, language);
			}
			else if (statement is Set.Statements.HasSignStatement)
			{
				return new HasSignStatement(statement as Set.Statements.HasSignStatement, language);
			}
			else if (statement is Modules.Classification.Statements.IsStatement)
			{
				return new IsStatement(statement as Modules.Classification.Statements.IsStatement, language);
			}
			else if (statement is Set.Statements.SignValueStatement)
			{
				return new SignValueStatement(statement as Set.Statements.SignValueStatement, language);
			}
			else if (statement is Mathematics.Statements.ComparisonStatement)
			{
				return new ComparisonStatement(statement as Mathematics.Statements.ComparisonStatement, language);
			}
			else if (statement is Processes.Statements.ProcessesStatement)
			{
				return new ProcessesStatement(statement as Processes.Statements.ProcessesStatement, language);
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
				if (statementNode.Statement is Set.Statements.HasPartStatement)
				{
					return new HasPartStatement(statementNode.Statement as Set.Statements.HasPartStatement, language);
				}
				else if (statementNode.Statement is Set.Statements.GroupStatement)
				{
					return new GroupStatement(statementNode.Statement as Set.Statements.GroupStatement, language);
				}
				else if (statementNode.Statement is Set.Statements.HasSignStatement)
				{
					return new HasSignStatement(statementNode.Statement as Set.Statements.HasSignStatement, language);
				}
				else if (statementNode.Statement is Modules.Classification.Statements.IsStatement)
				{
					return new IsStatement(statementNode.Statement as Modules.Classification.Statements.IsStatement, language);
				}
				else if (statementNode.Statement is Set.Statements.SignValueStatement)
				{
					return new SignValueStatement(statementNode.Statement as Set.Statements.SignValueStatement, language);
				}
				else if (statementNode.Statement is Mathematics.Statements.ComparisonStatement)
				{
					return new ComparisonStatement(statementNode.Statement as Mathematics.Statements.ComparisonStatement, language);
				}
				else if (statementNode.Statement is Processes.Statements.ProcessesStatement)
				{
					return new ProcessesStatement(statementNode.Statement as Processes.Statements.ProcessesStatement, language);
				}
			}

			throw new NotSupportedException(treeNode.GetType().FullName);
		}
	}
}
