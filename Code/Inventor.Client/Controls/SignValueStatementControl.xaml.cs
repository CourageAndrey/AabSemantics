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

			_groupConcept.Header = language.Ui.Editing.PropertyConcept;
			_groupSign.Header = language.Ui.Editing.PropertySign;
			_groupValue.Header = language.Ui.Editing.PropertyValue;
		}

		public ViewModels.SignValueStatement EditValue
		{
			get { return _contextControl.DataContext as ViewModels.SignValueStatement; }
			set { _contextControl.DataContext = value; }
		}
	}
}
