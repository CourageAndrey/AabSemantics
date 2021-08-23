using System.Linq;

using Inventor.Client.ViewModels;
using Inventor.Core;

namespace Inventor.Client.Controls
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

			_groupID.Header = language.Ui.Editing.PropertyID;
			_groupAncestor.Header = language.Ui.Editing.PropertyAncestor;
			_groupDescendant.Header = language.Ui.Editing.PropertyDescendant;
		}

		public StatementViewModel Statement
		{
			get { return _contextControl.DataContext as ViewModels.Statements.IsStatement; }
			set
			{
				_contextControl.DataContext = value;
				_idControl.IsReadOnly = value.BoundStatement?.Context is Core.Base.SystemContext;
			}
		}
	}
}
