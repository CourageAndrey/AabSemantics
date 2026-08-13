using System.Linq;
using System.Windows.Controls;

using AabSemantics.Extensions.WPF.ViewModels;
using AabSemantics.Metadata;

namespace AabSemantics.Extensions.WPF.Controls
{
	/// <summary>Editor control for custom statements.</summary>
	public partial class CustomStatementControl : IStatementEditor
	{
		/// <summary>Creates the control.</summary>
		public CustomStatementControl()
		{
			InitializeComponent();

			_comboBoxType.MakeAutoComplete();
		}

		/// <summary>Fills the control's pick lists and localizes its captions.</summary>
		/// <param name="semanticNetwork">Network supplying the selectable concepts.</param>
		/// <param name="language">Language the control is localized in.</param>
		public void Initialize(ISemanticNetwork semanticNetwork, ILanguage language)
		{
			_comboBoxType.ItemsSource = Repositories.CustomStatements.Values;

			var wrappedConcepts = semanticNetwork.Concepts.Select(c => new ConceptItem(c, language)).ToList();
			_columnConcept.ItemsSource = wrappedConcepts;

			var languageEditing = language.GetExtension<IWpfUiModule>().Ui.Editing;
			_groupID.Header = languageEditing.PropertyID;
			_groupType.Header = languageEditing.PropertyType;
			_groupConcepts.Header = languageEditing.PropertyConcepts;
			_columnKey.Header = languageEditing.PropertyKey;
			_columnConcept.Header = languageEditing.PropertyConcept;
		}

		/// <summary>The statement being edited.</summary>
		public StatementViewModel Statement
		{
			get { return _contextControl.DataContext as ViewModels.Statements.CustomStatement; }
			set
			{
				_contextControl.DataContext = value;
				_idControl.IsReadOnly = value.BoundStatement?.Context is Contexts.SystemContext;
			}
		}

		private void _selectedTypeChanged(object sender, SelectionChangedEventArgs e)
		{
			var statement = (ViewModels.Statements.CustomStatement) _contextControl.DataContext;
			statement.Concepts.Clear();

			var newType = _comboBoxType.SelectedItem as CustomStatementDefinition;
			if (newType != null)
			{
				foreach (var concept in newType.Concepts)
				{
					statement.Concepts.Add(new ConceptWithKey(concept, null));
				}
			}
		}
	}
}
