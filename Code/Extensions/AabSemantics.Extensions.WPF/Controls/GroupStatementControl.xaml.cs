using System.Linq;

using AabSemantics.Extensions.WPF.ViewModels;

namespace AabSemantics.Extensions.WPF.Controls
{
	/// <summary>Editor control for group statements.</summary>
	public partial class GroupStatementControl : IStatementEditor
	{
		/// <summary>Creates the control.</summary>
		public GroupStatementControl()
		{
			InitializeComponent();

			_comboBoxConcept.MakeAutoComplete();
			_comboBoxArea.MakeAutoComplete();
		}

		/// <summary>Fills the control's pick lists and localizes its captions.</summary>
		/// <param name="semanticNetwork">Network supplying the selectable concepts.</param>
		/// <param name="language">Language the control is localized in.</param>
		public void Initialize(ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var wrappedConcepts = semanticNetwork.Concepts.Select(c => new ConceptItem(c, language)).ToList();
			_comboBoxConcept.ItemsSource = wrappedConcepts;
			_comboBoxArea.ItemsSource = wrappedConcepts;

			var languageEditing = language.GetExtension<IWpfUiModule>().Ui.Editing;
			_groupID.Header = languageEditing.PropertyID;
			_groupArea.Header = languageEditing.PropertyArea;
			_groupConcept.Header = languageEditing.PropertyConcept;
		}

		/// <summary>The statement being edited.</summary>
		public StatementViewModel Statement
		{
			get { return _contextControl.DataContext as ViewModels.Statements.GroupStatement; }
			set
			{
				_contextControl.DataContext = value;
				_idControl.IsReadOnly = value.BoundStatement?.Context is Contexts.SystemContext;
			}
		}
	}
}
