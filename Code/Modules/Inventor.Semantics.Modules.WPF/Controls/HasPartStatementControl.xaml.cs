using System.Linq;

using Inventor.Semantics.Modules.WPF.ViewModels;

namespace Inventor.Semantics.Modules.WPF.Controls
{
	public partial class HasPartStatementControl : IStatementEditor
	{
		public HasPartStatementControl()
		{
			InitializeComponent();

			_comboBoxWhole.MakeAutoComplete();
			_comboBoxPart.MakeAutoComplete();
		}

		public void Initialize(ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var wrappedConcepts = semanticNetwork.Concepts.Select(c => new ConceptItem(c, language)).ToList();
			_comboBoxWhole.ItemsSource = wrappedConcepts;
			_comboBoxPart.ItemsSource = wrappedConcepts;

			var languageEditing = language.GetExtension<IWpfUiModule>().Ui.Editing;
			_groupID.Header = languageEditing.PropertyID;
			_groupWhole.Header = languageEditing.PropertyAncestor;
			_groupPart.Header = languageEditing.PropertyDescendant;
		}

		public StatementViewModel Statement
		{
			get { return _contextControl.DataContext as ViewModels.Statements.HasPartStatement; }
			set
			{
				_contextControl.DataContext = value;
				_idControl.IsReadOnly = value.BoundStatement?.Context is Semantics.Contexts.SystemContext;
			}
		}
	}
}
