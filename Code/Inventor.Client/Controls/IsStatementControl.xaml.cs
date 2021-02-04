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
		}

		public void Initialize(IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var wrappedConcepts = knowledgeBase.Concepts.Select(c => new ConceptItem(c, language)).ToList();
			_comboBoxAncestor.ItemsSource = wrappedConcepts;
			_comboBoxDescendant.ItemsSource = wrappedConcepts;

			_groupAncestor.Header = language.Ui.Editing.PropertyAncestor;
			_groupDescendant.Header = language.Ui.Editing.PropertyDescendant;
		}

		public StatementViewModel Statement
		{
			get { return _contextControl.DataContext as ViewModels.Statements.IsStatement; }
			set { _contextControl.DataContext = value; }
		}
	}
}
