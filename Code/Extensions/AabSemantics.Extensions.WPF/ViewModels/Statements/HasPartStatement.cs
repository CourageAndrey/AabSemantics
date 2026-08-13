using System.Windows;

using AabSemantics.Extensions.WPF.Controls;
using AabSemantics.Extensions.WPF.Dialogs;
using AabSemantics.Modules.Set.Localization;

namespace AabSemantics.Extensions.WPF.ViewModels.Statements
{
	/// <summary>Editable view over the "has part" statement.</summary>
	public class HasPartStatement : StatementViewModel<Modules.Set.Statements.HasPartStatement>
	{
		#region Properties

		/// <summary>The containing concept.</summary>
		public ConceptItem Whole
		{ get; set; }

		/// <summary>The contained concept.</summary>
		public ConceptItem Part
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty view model for a new statement.</summary>
		/// <param name="language">Language used to render the statement's text.</param>
		public HasPartStatement(ILanguage language)
			: this(string.Empty, null, null, language)
		{ }

		/// <summary>Creates a view model bound to an existing statement.</summary>
		/// <param name="statement">Statement to edit.</param>
		/// <param name="language">Language used to render the statement's text.</param>
		public HasPartStatement(Modules.Set.Statements.HasPartStatement statement, ILanguage language)
			: this(statement.ID, new ConceptItem(statement.Whole, language), new ConceptItem(statement.Part, language), language)
		{
			BoundObject = statement;
		}

		/// <summary>Creates a view model from explicit values.</summary>
		/// <param name="id">Identifier of the edited statement; empty when creating a new one.</param>
		/// <param name="whole">The containing concept.</param>
		/// <param name="part">The contained concept.</param>
		/// <param name="language">Language used to render the statement's text.</param>
		public HasPartStatement(string id, ConceptItem whole, ConceptItem part, ILanguage language)
			: base(id, language)
		{
			Whole = whole;
			Part = part;
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
			var control = new HasPartStatementControl
			{
				Statement = this,
			};
			control.Initialize(semanticNetwork, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.GetExtension<ILanguageSetModule>().Statements.Names.Composition,
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
		protected override AabSemantics.Modules.Set.Statements.HasPartStatement CreateStatementImplementation()
		{
			return new AabSemantics.Modules.Set.Statements.HasPartStatement(ID, Whole.Concept, Part.Concept);
		}

		/// <summary>Writes the edited values onto the bound statement.</summary>
		public override void ApplyUpdate()
		{
			BoundObject.Update(ID, Whole.Concept, Part.Concept);
		}

		#endregion

		/// <summary>Copies the view model.</summary>
		/// <returns>An independent copy.</returns>
		public override StatementViewModel Clone()
		{
			return new HasPartStatement(ID, Whole, Part, _language);
		}
	}
}
