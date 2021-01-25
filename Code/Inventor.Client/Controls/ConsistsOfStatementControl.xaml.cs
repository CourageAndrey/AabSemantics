using System.Linq;

using Inventor.Client.ViewModels;
using Inventor.Core;

namespace Inventor.Client.Controls
{
	public partial class ConsistsOfStatementControl : IStatementEditor
	{
		public ConsistsOfStatementControl()
		{
			InitializeComponent();
		}

		public void Initialize(IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var wrappedConcepts = knowledgeBase.Concepts.Select(c => new ConceptItem(c, language)).ToList();
			_comboBoxParent.ItemsSource = wrappedConcepts;
			_comboBoxChild.ItemsSource = wrappedConcepts;

			_groupParent.Header = language.Ui.Editing.PropertyParent;
			_groupChild.Header = language.Ui.Editing.PropertyChild;
		}

		public ViewModels.Statements.ConsistsOfStatement EditValue
		{
			get { return _contextControl.DataContext as ViewModels.Statements.ConsistsOfStatement; }
			set { _contextControl.DataContext = value; }
		}

		public StatementViewModel Statement
		{ get { return EditValue; } }
	}
}
