using System.Linq;

using Inventor.Semantics.Modules.WPF.ViewModels;

namespace Inventor.Semantics.Modules.WPF.Controls
{
	public partial class GroupStatementControl : IStatementEditor
	{
		public GroupStatementControl()
		{
			InitializeComponent();

			_comboBoxConcept.MakeAutoComplete();
			_comboBoxArea.MakeAutoComplete();
		}

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

		public StatementViewModel Statement
		{
			get { return _contextControl.DataContext as ViewModels.Statements.GroupStatement; }
			set
			{
				_contextControl.DataContext = value;
				_idControl.IsReadOnly = value.BoundStatement?.Context is Semantics.Contexts.SystemContext;
			}
		}
	}
}
