using Inventor.Core;

namespace Inventor.Client.Controls
{
	public partial class HasSignStatementControl
	{
		public HasSignStatementControl()
		{
			InitializeComponent();
		}

		public void Initialize(IKnowledgeBase knowledgeBase, ILanguage language)
		{
			_comboBoxConcept.ItemsSource = knowledgeBase.Concepts;
			_comboBoxSign.ItemsSource = knowledgeBase.Concepts;

			_groupConcept.Header = language.Ui.Editing.PropertyConcept;
			_groupSign.Header = language.Ui.Editing.PropertySign;
		}

		public ViewModels.HasSignStatement EditValue
		{
			get { return _contextControl.DataContext as ViewModels.HasSignStatement; }
			set { _contextControl.DataContext = value; }
		}
	}
}
