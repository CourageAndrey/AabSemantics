using System.Linq;

using Inventor.Client.ViewModels;
using Inventor.Core;

namespace Inventor.Client.Controls
{
	public partial class GroupStatementControl : IStatementEditor
	{
		public GroupStatementControl()
		{
			InitializeComponent();
		}

		public void Initialize(IKnowledgeBase knowledgeBase, ILanguage language)
		{
			var wrappedConcepts = knowledgeBase.Concepts.Select(c => new ConceptItem(c, language)).ToList();
			_comboBoxConcept.ItemsSource = wrappedConcepts;
			_comboBoxArea.ItemsSource = wrappedConcepts;

			_groupArea.Header = language.Ui.Editing.PropertyArea;
			_groupConcept.Header = language.Ui.Editing.PropertyConcept;
		}

		public StatementViewModel Statement
		{
			get { return _contextControl.DataContext as ViewModels.Statements.GroupStatement; }
			set { _contextControl.DataContext = value; }
		}
	}
}
