using Inventor.Core;

namespace Inventor.Client.Controls
{
	public partial class SignValueStatementControl
	{
		public SignValueStatementControl()
		{
			InitializeComponent();
		}

		public void Initialize(IKnowledgeBase knowledgeBase, ILanguage language)
		{
			_comboBoxConcept.ItemsSource = knowledgeBase.Concepts;
			_comboBoxSign.ItemsSource = knowledgeBase.Concepts;
			_comboBoxValue.ItemsSource = knowledgeBase.Concepts;
		}

		public ViewModels.SignValueStatement EditValue
		{
			get { return _contextControl.DataContext as ViewModels.SignValueStatement; }
			set { _contextControl.DataContext = value; }
		}
	}
}
