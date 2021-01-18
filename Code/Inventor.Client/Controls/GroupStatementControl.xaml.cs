using System.Linq;

using Inventor.Client.ViewModels;
using Inventor.Core;

namespace Inventor.Client.Controls
{
	public partial class GroupStatementControl
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

		public ViewModels.GroupStatement EditValue
		{
			get { return _contextControl.DataContext as ViewModels.GroupStatement; }
			set { _contextControl.DataContext = value; }
		}
	}
}
