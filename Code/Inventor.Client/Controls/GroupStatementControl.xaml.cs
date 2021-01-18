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
			_comboBoxConcept.ItemsSource = knowledgeBase.Concepts;
			_comboBoxArea.ItemsSource = knowledgeBase.Concepts;
		}

		public ViewModels.GroupStatement EditValue
		{
			get { return _contextControl.DataContext as ViewModels.GroupStatement; }
			set { _contextControl.DataContext = value; }
		}
	}
}
