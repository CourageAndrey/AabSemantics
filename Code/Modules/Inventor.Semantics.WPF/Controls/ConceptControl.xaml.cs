using System.Windows;
using System.Windows.Controls;

using Inventor.Semantics.WPF.ViewModels;

namespace Inventor.Semantics.WPF.Controls
{
	public partial class ConceptControl
	{
		public ConceptControl()
		{
			InitializeComponent();
		}

		public void Initialize(ISemanticNetwork semanticNetwork, ILanguage language)
		{
			var languageEditing = language.GetExtension<IWpfUiModule>().Ui.Editing;
			_groupID.Header = languageEditing.PropertyID;
			_groupName.Header = languageEditing.PropertyName;
			_groupHint.Header = languageEditing.PropertyHint;
			_groupAttributes.Header = languageEditing.PropertyAttributes;

			_nameControl.Localize(language);
			_hintControl.Localize(language);
		}

		public Concept EditValue
		{
			get { return _concept; }
			set
			{
				_contextControl.DataContext = _concept = value;

				_idControl.IsReadOnly = value.BoundObject is Semantics.Concepts.SystemConcept;
				_nameControl.IsEnabled = value.Name != null;
				_hintControl.IsEnabled = value.Hint != null;
				_groupAttributes.IsEnabled = value.Attributes != null;
			}
		}

		private Concept _concept;

		private void attributeChecked(object sender, RoutedEventArgs e)
		{
			var checkBox = (CheckBox) sender;
			var attributeViewModel = (ConceptAttribute) checkBox.Tag;

			if (checkBox.IsChecked == true)
			{
				// first attribute is always None
				if (attributeViewModel.Value != null)
				{
					_concept.Attributes[0].SwitchOff();
				}
				else
				{
					for (int a = 1; a < _concept.Attributes.Count; a++)
					{
						_concept.Attributes[a].SwitchOff();
					}
				}
			}
		}
	}
}
