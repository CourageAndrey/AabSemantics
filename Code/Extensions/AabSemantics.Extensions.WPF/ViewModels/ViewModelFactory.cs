using System;

using AabSemantics.Extensions.WPF.TreeNodes;
using AabSemantics.Extensions.WPF.ViewModels.Statements;

namespace AabSemantics.Extensions.WPF.ViewModels
{
	/// <summary>Creates the view models the editing dialogs bind to.</summary>
	public interface IViewModelFactory
	{
		/// <summary>Creates an empty view model for a knowledge type, ready to define a new item.</summary>
		/// <param name="type">Concept or statement type to create a view model for.</param>
		/// <param name="language">Language the dialog is localized in.</param>
		/// <returns>The view model.</returns>
		IKnowledgeViewModel CreateByCoreType(Type type, ILanguage language);

		/// <summary>Creates a view model bound to an existing statement.</summary>
		/// <param name="statement">Statement to edit.</param>
		/// <param name="language">Language the dialog is localized in.</param>
		/// <returns>The view model.</returns>
		StatementViewModel CreateStatementByInstance(IStatement statement, ILanguage language);

		/// <summary>Creates a view model bound to the item a tree node stands for.</summary>
		/// <param name="treeNode">Tree node whose item is edited.</param>
		/// <param name="language">Language the dialog is localized in.</param>
		/// <returns>The view model.</returns>
		IKnowledgeViewModel CreateByTreeNode(ExtendedTreeNode treeNode, ILanguage language);
	}

	/// <summary>Default <see cref="IViewModelFactory"/>; override its methods to support custom types.</summary>
	public class ViewModelFactory : IViewModelFactory
	{
		/// <summary>Creates an empty view model for a knowledge type, ready to define a new item.</summary>
		/// <param name="type">Concept or statement type to create a view model for.</param>
		/// <param name="language">Language the dialog is localized in.</param>
		/// <returns>The view model.</returns>
		public virtual IKnowledgeViewModel CreateByCoreType(Type type, ILanguage language)
		{
			if (type == typeof(Concepts.Concept))
			{
				return new Concept(language);
			}
			else if (type == typeof(Modules.Set.Statements.HasPartStatement))
			{
				return new HasPartStatement(language);
			}
			else if (type == typeof(Modules.Set.Statements.GroupStatement))
			{
				return new GroupStatement(language);
			}
			else if (type == typeof(Modules.Set.Statements.HasSignStatement))
			{
				return new HasSignStatement(language);
			}
			else if (type == typeof(Modules.Classification.Statements.IsStatement))
			{
				return new IsStatement(language);
			}
			else if (type == typeof(Modules.Set.Statements.SignValueStatement))
			{
				return new SignValueStatement(language);
			}
			else if (type == typeof(Modules.Mathematics.Statements.ComparisonStatement))
			{
				return new ComparisonStatement(language);
			}
			else if (type == typeof(Modules.Processes.Statements.ProcessesStatement))
			{
				return new ProcessesStatement(language);
			}
			else if (type == typeof(AabSemantics.Statements.CustomStatement))
			{
				return new CustomStatement(language);
			}
			else
			{
				throw new NotSupportedException(type.FullName);
			}
		}

		/// <summary>Creates a view model bound to an existing statement.</summary>
		/// <param name="statement">Statement to edit.</param>
		/// <param name="language">Language the dialog is localized in.</param>
		/// <returns>The view model.</returns>
		public virtual StatementViewModel CreateStatementByInstance(IStatement statement, ILanguage language)
		{
			if (statement is Modules.Set.Statements.HasPartStatement)
			{
				return new HasPartStatement(statement as Modules.Set.Statements.HasPartStatement, language);
			}
			else if (statement is Modules.Set.Statements.GroupStatement)
			{
				return new GroupStatement(statement as Modules.Set.Statements.GroupStatement, language);
			}
			else if (statement is Modules.Set.Statements.HasSignStatement)
			{
				return new HasSignStatement(statement as Modules.Set.Statements.HasSignStatement, language);
			}
			else if (statement is Modules.Classification.Statements.IsStatement)
			{
				return new IsStatement(statement as Modules.Classification.Statements.IsStatement, language);
			}
			else if (statement is Modules.Set.Statements.SignValueStatement)
			{
				return new SignValueStatement(statement as Modules.Set.Statements.SignValueStatement, language);
			}
			else if (statement is Modules.Mathematics.Statements.ComparisonStatement)
			{
				return new ComparisonStatement(statement as Modules.Mathematics.Statements.ComparisonStatement, language);
			}
			else if (statement is Modules.Processes.Statements.ProcessesStatement)
			{
				return new ProcessesStatement(statement as Modules.Processes.Statements.ProcessesStatement, language);
			}
			else if (statement is AabSemantics.Statements.CustomStatement)
			{
				return new CustomStatement(statement as AabSemantics.Statements.CustomStatement, language);
			}
			else
			{
				throw new NotSupportedException(statement.GetType().FullName);
			}
		}

		/// <summary>Creates a view model bound to the item a tree node stands for.</summary>
		/// <param name="treeNode">Tree node whose item is edited.</param>
		/// <param name="language">Language the dialog is localized in.</param>
		/// <returns>The view model.</returns>
		public virtual IKnowledgeViewModel CreateByTreeNode(ExtendedTreeNode treeNode, ILanguage language)
		{
			var conceptNode = treeNode as ConceptNode;
			var statementNode = treeNode as StatementNode;

			if (conceptNode != null)
			{
				return new Concept(conceptNode.Concept as Concepts.Concept);
			}
			else if (statementNode != null)
			{
				if (statementNode.Statement is Modules.Set.Statements.HasPartStatement)
				{
					return new HasPartStatement(statementNode.Statement as Modules.Set.Statements.HasPartStatement, language);
				}
				else if (statementNode.Statement is Modules.Set.Statements.GroupStatement)
				{
					return new GroupStatement(statementNode.Statement as Modules.Set.Statements.GroupStatement, language);
				}
				else if (statementNode.Statement is Modules.Set.Statements.HasSignStatement)
				{
					return new HasSignStatement(statementNode.Statement as Modules.Set.Statements.HasSignStatement, language);
				}
				else if (statementNode.Statement is Modules.Classification.Statements.IsStatement)
				{
					return new IsStatement(statementNode.Statement as Modules.Classification.Statements.IsStatement, language);
				}
				else if (statementNode.Statement is Modules.Set.Statements.SignValueStatement)
				{
					return new SignValueStatement(statementNode.Statement as Modules.Set.Statements.SignValueStatement, language);
				}
				else if (statementNode.Statement is Modules.Mathematics.Statements.ComparisonStatement)
				{
					return new ComparisonStatement(statementNode.Statement as Modules.Mathematics.Statements.ComparisonStatement, language);
				}
				else if (statementNode.Statement is Modules.Processes.Statements.ProcessesStatement)
				{
					return new ProcessesStatement(statementNode.Statement as Modules.Processes.Statements.ProcessesStatement, language);
				}
				else if (statementNode.Statement is AabSemantics.Statements.CustomStatement)
				{
					return new CustomStatement(statementNode.Statement as AabSemantics.Statements.CustomStatement, language);
				}
			}

			throw new NotSupportedException(treeNode.GetType().FullName);
		}
	}
}
