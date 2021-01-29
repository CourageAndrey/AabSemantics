using Inventor.Core;

namespace Inventor.Client.Controls
{
	public partial class ConceptControl
	{
		public ConceptControl()
		{
			InitializeComponent();
		}

		public void Initialize(IKnowledgeBase knowledgeBase, ILanguage language)
		{
			_groupName.Header = language.Ui.Editing.PropertyName;
			_groupHint.Header = language.Ui.Editing.PropertyHint;

			_nameControl.Localize(language);
			_hintControl.Localize(language);
		}

		public ViewModels.Concept EditValue
		{
			get { return _contextControl.DataContext as ViewModels.Concept; }
			set
			{
				_contextControl.DataContext = value;
				_nameControl.IsEnabled = value.Name != null;
				_hintControl.IsEnabled = value.Hint != null;
			}
		}
	}
}
