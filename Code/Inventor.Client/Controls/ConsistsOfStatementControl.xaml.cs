using Inventor.Core;

namespace Inventor.Client.Controls
{
	public partial class ConsistsOfStatementControl
	{
		public ConsistsOfStatementControl()
		{
			InitializeComponent();
		}

		public void Initialize(IKnowledgeBase knowledgeBase, ILanguage language)
		{
			_comboBoxParent.ItemsSource = knowledgeBase.Concepts;
			_comboBoxChild.ItemsSource = knowledgeBase.Concepts;
		}

		public ViewModels.ConsistsOfStatement EditValue
		{
			get { return _contextControl.DataContext as ViewModels.ConsistsOfStatement; }
			set { _contextControl.DataContext = value; }
		}
	}
}
