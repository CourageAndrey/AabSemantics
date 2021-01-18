using System.Linq;

using Inventor.Client.ViewModels;
using Inventor.Core;

namespace Inventor.Client.Controls
{
	public partial class IsStatementControl
	{
		public IsStatementControl()
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

		public ViewModels.IsStatement EditValue
		{
			get { return _contextControl.DataContext as ViewModels.IsStatement; }
			set { _contextControl.DataContext = value; }
		}
	}
}
