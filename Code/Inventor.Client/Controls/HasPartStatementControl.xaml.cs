using System.Linq;

using Inventor.Client.ViewModels;
using Inventor.Core;

namespace Inventor.Client.Controls
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

			_groupID.Header = language.Ui.Editing.PropertyID;
			_groupWhole.Header = language.Ui.Editing.PropertyAncestor;
			_groupPart.Header = language.Ui.Editing.PropertyDescendant;
		}

		public StatementViewModel Statement
		{
			get { return _contextControl.DataContext as ViewModels.Statements.HasPartStatement; }
			set
			{
				_contextControl.DataContext = value;
				_idControl.IsReadOnly = value.BoundStatement?.Context is Core.Contexts.SystemContext;
			}
		}
	}
}
