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
			_comboBoxParent.ItemsSource = knowledgeBase.Concepts;
			_comboBoxChild.ItemsSource = knowledgeBase.Concepts;

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
