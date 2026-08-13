using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using AabSemantics.Extensions.WPF.Controls;
using AabSemantics.Extensions.WPF.Dialogs;
using AabSemantics.Modules.Classification.Localization;

namespace AabSemantics.Extensions.WPF.ViewModels.Statements
{
	/// <summary>Editable view over a run-time declared statement.</summary>
	public class CustomStatement : StatementViewModel<AabSemantics.Statements.CustomStatement>
	{
		#region Properties

		/// <summary>Identifier of the declared statement kind.</summary>
		public string Type
		{ get; set; }

		/// <summary>Concepts filling the kind's roles.</summary>
		public ObservableCollection<ConceptWithKey> Concepts
		{ get; set; }

		#endregion

		#region Constructors

		/// <summary>Creates an empty view model for a new statement.</summary>
		/// <param name="language">Language used to render the statement's text.</param>
		public CustomStatement(ILanguage language)
			: this(string.Empty, string.Empty, new ObservableCollection<ConceptWithKey>(), language)
		{ }

		/// <summary>Creates a view model bound to an existing statement.</summary>
		/// <param name="statement">Statement to edit.</param>
		/// <param name="language">Language used to render the statement's text.</param>
		public CustomStatement(AabSemantics.Statements.CustomStatement statement, ILanguage language)
			: this(statement.ID, statement.Type, new ObservableCollection<ConceptWithKey>(statement.Concepts.Select(c => new ConceptWithKey(c.Key, new ConceptItem(c.Value, language)))), language)
		{
			BoundObject = statement;
		}

		/// <summary>Creates a view model from explicit values.</summary>
		/// <param name="id">Identifier of the edited statement; empty when creating a new one.</param>
		/// <param name="type">Identifier of the declared statement kind.</param>
		/// <param name="concepts">Concepts filling the kind's roles; copied into the view model.</param>
		/// <param name="language">Language used to render the statement's text.</param>
		public CustomStatement(string id, string type, ObservableCollection<ConceptWithKey> concepts, ILanguage language)
			: base(id, language)
		{
			Type = type;
			Concepts = new ObservableCollection<ConceptWithKey>(concepts);
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
			var control = new CustomStatementControl
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
		protected override AabSemantics.Statements.CustomStatement CreateStatementImplementation()
		{
			return new AabSemantics.Statements.CustomStatement(ID, Type, Concepts.ToDictionary(c => c.Key, c => c.Concept.Concept));
		}

		/// <summary>Writes the edited values onto the bound statement.</summary>
		public override void ApplyUpdate()
		{
			BoundObject.Update(ID, Concepts.ToDictionary(c => c.Key, c => c.Concept.Concept));
		}

		#endregion

		/// <summary>Copies the view model.</summary>
		/// <returns>An independent copy.</returns>
		public override StatementViewModel Clone()
		{
			return new CustomStatement(ID, Type, Concepts, _language);
		}
	}
}
