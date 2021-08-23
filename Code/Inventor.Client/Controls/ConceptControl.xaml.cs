using System.Windows;
using System.Windows.Controls;

using Inventor.Client.ViewModels;
using Inventor.Core;

namespace Inventor.Client.Controls
{
	public partial class ConceptControl
	{
		public ConceptControl()
		{
			InitializeComponent();
		}

		public void Initialize(ISemanticNetwork semanticNetwork, ILanguage language)
		{
			_groupID.Header = language.Ui.Editing.PropertyID;
			_groupName.Header = language.Ui.Editing.PropertyName;
			_groupHint.Header = language.Ui.Editing.PropertyHint;
			_groupAttributes.Header = language.Ui.Editing.PropertyAttributes;

			_nameControl.Localize(language);
			_hintControl.Localize(language);
		}

		public Concept EditValue
		{
			get { return _concept; }
			set
			{
				_contextControl.DataContext = _concept = value;

				_idControl.IsReadOnly = value.BoundObject is Core.Base.SystemConcept;
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
