using System.Linq;

using Inventor.Semantics.Extensions.WPF.ViewModels;

namespace Inventor.Semantics.Extensions.WPF.Controls
{
	public partial class IsStatementControl : IStatementEditor
	{
		public IsStatementControl()
		{
			InitializeComponent();

			_comboBoxAncestor.MakeAutoComplete();
			_comboBoxDescendant.MakeAutoComplete();
		}

		public void Initialize(ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var wrappedConcepts = semanticNetwork.Concepts.Select(c => new ConceptItem(c, language)).ToList();
			_comboBoxAncestor.ItemsSource = wrappedConcepts;
			_comboBoxDescendant.ItemsSource = wrappedConcepts;

			var languageEditing = language.GetExtension<IWpfUiModule>().Ui.Editing;
			_groupID.Header = languageEditing.PropertyID;
			_groupAncestor.Header = languageEditing.PropertyAncestor;
			_groupDescendant.Header = languageEditing.PropertyDescendant;
		}

		public StatementViewModel Statement
		{
			get { return _contextControl.DataContext as ViewModels.Statements.IsStatement; }
			set
			{
				_contextControl.DataContext = value;
				_idControl.IsReadOnly = value.BoundStatement?.Context is Contexts.SystemContext;
			}
		}
	}
}
