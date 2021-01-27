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
			_comboBoxWhole.ItemsSource = wrappedConcepts;
			_comboBoxPart.ItemsSource = wrappedConcepts;

			_groupWhole.Header = language.Ui.Editing.PropertyAncestor;
			_groupPart.Header = language.Ui.Editing.PropertyDescendant;
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
