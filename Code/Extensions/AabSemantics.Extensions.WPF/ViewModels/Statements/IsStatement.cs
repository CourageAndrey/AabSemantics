using System.Windows;

using AabSemantics.Extensions.WPF.Controls;
using AabSemantics.Extensions.WPF.Dialogs;
using AabSemantics.Modules.Classification.Localization;

namespace AabSemantics.Extensions.WPF.ViewModels.Statements
{
	/// <summary>Editable view over the "is a" statement.</summary>
	public class IsStatement : StatementViewModel<Modules.Classification.Statements.IsStatement>
	{
		#region Properties

		/// <summary>The more general concept.</summary>
		public ConceptItem Ancestor
		{ get; set; }

		/// <summary>The more specific concept.</summary>
		public ConceptItem Descendant
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty view model for a new statement.</summary>
		/// <param name="language">Language used to render the statement's text.</param>
		public IsStatement(ILanguage language)
			: this(string.Empty, null, null, language)
		{ }

		/// <summary>Creates a view model bound to an existing statement.</summary>
		/// <param name="statement">Statement to edit.</param>
		/// <param name="language">Language used to render the statement's text.</param>
		public IsStatement(Modules.Classification.Statements.IsStatement statement, ILanguage language)
			: this(statement.ID, new ConceptItem(statement.Ancestor, language), new ConceptItem(statement.Descendant, language), language)
		{
			BoundObject = statement;
		}

		/// <summary>Creates a view model from explicit values.</summary>
		/// <param name="id">Identifier of the edited statement; empty when creating a new one.</param>
		/// <param name="ancestor">The more general concept.</param>
		/// <param name="descendant">The more specific concept.</param>
		/// <param name="language">Language used to render the statement's text.</param>
		public IsStatement(string id, ConceptItem ancestor, ConceptItem descendant, ILanguage language)
			: base(id, language)
		{
			Ancestor = ancestor;
			Descendant = descendant;
		}

		#endregion

		#region Implementation of IViewModel

		/// <summary>Builds the dialog used to edit this statement.</summary>
		/// <param name="owner">Window the dialog belongs to.</param>
		/// <param name="semanticNetwork">Network supplying the pick lists.</param>
		/// <param name="language">Language the dialog is localized in.</param>
		/// <returns>An unshown dialog.</returns>
		public override Window CreateEditDialog(Window owner, ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var control = new IsStatementControl
			{
				Statement = this,
			};
			control.Initialize(semanticNetwork, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.GetExtension<ILanguageClassificationModule>().Statements.Names.Classification,
				SizeToContent = SizeToContent.WidthAndHeight,
				MinWidth = 200,
				MinHeight = 100,
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
			};
			dialog.Localize(language);
			return dialog;
		}

		/// <summary>Builds the statement from the edited values.</summary>
		/// <returns>The created statement.</returns>
		protected override Modules.Classification.Statements.IsStatement CreateStatementImplementation()
		{
			return new Modules.Classification.Statements.IsStatement(ID, Ancestor.Concept, Descendant.Concept);
		}

		/// <summary>Writes the edited values onto the bound statement.</summary>
		public override void ApplyUpdate()
		{
			BoundObject.Update(ID, Ancestor.Concept, Descendant.Concept);
		}

		#endregion

		/// <summary>Copies the view model.</summary>
		/// <returns>An independent copy.</returns>
		public override StatementViewModel Clone()
		{
			return new IsStatement(ID, Ancestor, Descendant, _language);
		}
	}
}
