using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Inventor.Semantics.Metadata;

namespace Inventor.Semantics.WPF.Dialogs
{
	public partial class SelectModulesDialog
	{
		public SelectModulesDialog()
		{
			InitializeComponent();
		}

		public bool IsReadOnly
		{
			get { return !_groupBox.IsEnabled; }
			set
			{
				_groupBox.IsEnabled = !value;
				_buttonOk.Visibility = value
					? Visibility.Collapsed
					: Visibility.Visible;
			}
		}

		public void Initialize(ILanguage language)
		{
			_groupBox.Children.Clear();
			foreach (var module in Repositories.Modules.Values)
			{
				if (!(module is WpfUiModule))
				{
					var checkBox = new CheckBox
					{
						Margin = new Thickness(5),
						Content = module.Name,
						Tag = module,
						IsChecked = true,
					};
					checkBox.Checked += moduleChecked;
					checkBox.Unchecked += moduleUnchecked;
					_groupBox.Children.Add(checkBox);
				}
			}

			Title = language.GetExtension<IWpfUiModule>().Ui.SelectModulesDialogHeader;
			_buttonOk.Content = language.GetExtension<IWpfUiModule>().Common.Ok;
			_buttonCancel.Content = language.GetExtension<IWpfUiModule>().Common.Cancel;
		}

		private void moduleChecked(object sender, RoutedEventArgs e)
		{
			if (_suppressModulesChainUpdate) return;

			var module = (IExtensionModule) ((CheckBox) sender).Tag;

			foreach (var checkBox in _groupBox.Children.OfType<CheckBox>())
			{
				if (sender != checkBox && module.Dependencies.Contains(((IExtensionModule) checkBox.Tag).Name))
				{
					checkBox.IsChecked = true;
				}
			}
		}

		private void moduleUnchecked(object sender, RoutedEventArgs e)
		{
			if (_suppressModulesChainUpdate) return;

			var module = (IExtensionModule) ((CheckBox) sender).Tag;

			foreach (var checkBox in _groupBox.Children.OfType<CheckBox>())
			{
				if (sender != checkBox && ((IExtensionModule) checkBox.Tag).Dependencies.Contains(module.Name))
				{
					checkBox.IsChecked = false;
				}
			}
		}

		public ICollection<IExtensionModule> SelectedModules
		{
			get
			{
				return _groupBox.Children
					.OfType<CheckBox>()
					.Where(checkBox => checkBox.IsChecked == true)
					.Select(checkBox => checkBox.Tag as IExtensionModule).ToList();
			}
			set
			{
				_suppressModulesChainUpdate = true;
				foreach (var checkBox in _groupBox.Children.OfType<CheckBox>())
				{
					checkBox.IsChecked = value.Any(module => module.Name == (string) checkBox.Content);
				}
				_suppressModulesChainUpdate = false;
			}
		}

		private bool _suppressModulesChainUpdate;

		private void okClick(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void cancelClick(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}
	}
}
