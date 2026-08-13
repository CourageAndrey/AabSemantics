using System.Windows;

namespace AabSemantics.Extensions.WPF
{
	/// <summary>
	/// An editable view over one knowledge item. It supplies its own edit dialog and knows how to
	/// write the edited values back, either as a new item or onto the existing one.
	/// </summary>
	public interface IKnowledgeViewModel
	{
		/// <summary>Builds the dialog used to edit this item.</summary>
		/// <param name="owner">Window the dialog belongs to.</param>
		/// <param name="semanticNetwork">Network the item belongs to; supplies the pick lists.</param>
		/// <param name="language">Language the dialog is localized in.</param>
		/// <returns>An unshown dialog.</returns>
		Window CreateEditDialog(Window owner, ISemanticNetwork semanticNetwork, ILanguage language);

		/// <summary>Creates the item from the edited values and adds it to the network.</summary>
		/// <param name="semanticNetwork">Network to add the item to.</param>
		/// <returns>The created item.</returns>
		object ApplyCreate(ISemanticNetwork semanticNetwork);

		/// <summary>Writes the edited values onto the already existing item.</summary>
		void ApplyUpdate();
	}

	/// <summary>Base view model for statements, untyped.</summary>
	public abstract class StatementViewModel : IKnowledgeViewModel
	{
		/// <summary>Identifier of the edited statement.</summary>
		public string ID
		{ get; set; }

		/// <summary>The statement being edited, or <c>null</c> while creating a new one.</summary>
		public abstract IStatement BoundStatement
		{ get; }

		/// <summary>Builds the dialog used to edit this statement.</summary>
		/// <param name="owner">Window the dialog belongs to.</param>
		/// <param name="semanticNetwork">Network the statement belongs to.</param>
		/// <param name="language">Language the dialog is localized in.</param>
		/// <returns>An unshown dialog.</returns>
		public abstract Window CreateEditDialog(Window owner, ISemanticNetwork semanticNetwork, ILanguage language);

		/// <summary>Copies the view model, so an edit can be cancelled without touching the original.</summary>
		/// <returns>An independent copy.</returns>
		public abstract StatementViewModel Clone();

		/// <summary>Creates the statement from the edited values and adds it to the network.</summary>
		/// <param name="semanticNetwork">Network to add the statement to.</param>
		/// <returns>The created statement.</returns>
		public object ApplyCreate(ISemanticNetwork semanticNetwork)
		{
			var statement = CreateStatement();
			semanticNetwork.Statements.Add(statement);
			return statement;
		}

		/// <summary>Writes the edited values onto the already existing statement.</summary>
		public abstract void ApplyUpdate();

		/// <summary>Builds the statement from the edited values without adding it anywhere.</summary>
		/// <returns>The created statement.</returns>
		public abstract IStatement CreateStatement();
	}

	/// <summary>Base view model for statements of one concrete type.</summary>
	/// <typeparam name="StatementT">Statement type being edited.</typeparam>
	public abstract class StatementViewModel<StatementT> : StatementViewModel
		where StatementT : class, IStatement
	{
		/// <summary>The statement being edited, typed as the general interface.</summary>
		public override IStatement BoundStatement
		{ get { return BoundObject;} }

		/// <summary>The statement being edited, or <c>null</c> while creating a new one.</summary>
		public StatementT BoundObject
		{ get; protected set; }

		/// <summary>Language used to render the statement's text.</summary>
		protected readonly ILanguage _language;

		/// <summary>Initializes the view model.</summary>
		/// <param name="id">Identifier of the edited statement; empty when creating a new one.</param>
		/// <param name="language">Language used to render the statement's text.</param>
		protected StatementViewModel(string id, ILanguage language)
		{
			ID = id;
			_language = language;
		}

		/// <summary>
		/// Builds the statement and remembers it as the bound object, so a subsequent
		/// <see cref="StatementViewModel.ApplyUpdate"/> edits the same instance.
		/// </summary>
		/// <returns>The created statement.</returns>
		public override IStatement CreateStatement()
		{
			return BoundObject = CreateStatementImplementation();
		}

		/// <summary>Builds the statement in its concrete type.</summary>
		/// <returns>The created statement.</returns>
		protected abstract StatementT CreateStatementImplementation();

		/// <summary>
		/// Renders the statement as a plain affirmative sentence, which is what the tree and list
		/// controls display. Builds a throwaway statement when nothing is bound yet.
		/// </summary>
		/// <returns>The rendered text.</returns>
		public override string ToString()
		{
			var statement = BoundObject ?? CreateStatementImplementation();
			var text = statement.DescribeTrue();
			return TextRenders.PlainString.RenderText(text, _language).ToString();
		}
	}
}
