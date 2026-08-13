using System.Windows;

using AabSemantics.Extensions.WPF.Controls;
using AabSemantics.Extensions.WPF.Dialogs;
using AabSemantics.Modules.Set.Localization;

namespace AabSemantics.Extensions.WPF.ViewModels.Statements
{
	/// <summary>Editable view over the "has sign" statement.</summary>
	public class HasSignStatement : StatementViewModel<Modules.Set.Statements.HasSignStatement>
	{
		#region Properties

		/// <summary>The concept in question.</summary>
		public ConceptItem Concept
		{ get; set; }

		/// <summary>The sign concept.</summary>
		public ConceptItem Sign
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty view model for a new statement.</summary>
		/// <param name="language">Language used to render the statement's text.</param>
		public HasSignStatement(ILanguage language)
			: this(string.Empty, null, null, language)
		{ }

		/// <summary>Creates a view model bound to an existing statement.</summary>
		/// <param name="statement">Statement to edit.</param>
		/// <param name="language">Language used to render the statement's text.</param>
		public HasSignStatement(Modules.Set.Statements.HasSignStatement statement, ILanguage language)
			: this(statement.ID, new ConceptItem(statement.Concept, language), new ConceptItem(statement.Sign, language), language)
		{
			BoundObject = statement;
		}

		/// <summary>Creates a view model from explicit values.</summary>
		/// <param name="id">Identifier of the edited statement; empty when creating a new one.</param>
		/// <param name="concept">The concept in question.</param>
		/// <param name="sign">The sign concept.</param>
		/// <param name="language">Language used to render the statement's text.</param>
		public HasSignStatement(string id, ConceptItem concept, ConceptItem sign, ILanguage language)
			: base(id, language)
		{
			Concept = concept;
			Sign = sign;
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
			var control = new HasSignStatementControl
			{
				Statement = this,
			};
			control.Initialize(semanticNetwork, language);
			var dialog = new EditDialog
			{
				Owner = owner,
				Editor = control,
				Title = language.GetExtension<ILanguageSetModule>().Statements.Names.HasSign,
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
		protected override AabSemantics.Modules.Set.Statements.HasSignStatement CreateStatementImplementation()
		{
			return new AabSemantics.Modules.Set.Statements.HasSignStatement(ID, Concept.Concept, Sign.Concept);
		}

		/// <summary>Writes the edited values onto the bound statement.</summary>
		public override void ApplyUpdate()
		{
			BoundObject.Update(ID, Concept.Concept, Sign.Concept);
		}

		#endregion

		/// <summary>Copies the view model.</summary>
		/// <returns>An independent copy.</returns>
		public override StatementViewModel Clone()
		{
			return new HasSignStatement(ID, Concept, Sign, _language);
		}
	}
}
