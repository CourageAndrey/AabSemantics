using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

using Inventor.Core;

namespace Inventor.WPF
{
	public static class UiHelper
	{
		public static void ExecuteWithItem(this ItemsControl parentContainer, List<object> path, Action<TreeViewItem> code)
		{
			if (path.Count > 0)
			{
				var head = path.First();
				var tail = path.GetRange(1, path.Count - 1);
				var item = parentContainer.ItemContainerGenerator.ContainerFromItem(head) as TreeViewItem;
				if (item != null)
				{
					item.IsExpanded = true;
					if (item.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
					{
						item.ItemContainerGenerator.StatusChanged += delegate
						{
							ExecuteWithItem(item, tail, code);
						};
					}
					else
					{
						ExecuteWithItem(item, tail, code);
					}
				}
			}
			else
			{
				code(parentContainer as TreeViewItem);
			}
		}

		#region ComboBox autocomplete

		public static void MakeAutoComplete(this ComboBox comboBox)
		{
			comboBox.IsEditable = true;
			comboBox.IsTextSearchEnabled = false;
			comboBox.PreviewTextInput += (sender, e) => previewTextInput((ComboBox) sender, e);
			comboBox.PreviewKeyUp += (sender, e) => previewKeyUp((ComboBox) sender, e);
			DataObject.AddPastingHandler(comboBox, (sender, e) => pasting((ComboBox) sender, e));
		}

		private static void previewTextInput(ComboBox comboBox, TextCompositionEventArgs e)
		{
			comboBox.Tag = comboBox.Tag ?? comboBox.ItemsSource;

			comboBox.IsDropDownOpen = true;

			var items = comboBox.getOriginalItemSource();
			if (!string.IsNullOrEmpty(comboBox.Text))
			{
				string fullText = comboBox.insertFullText(e.Text);
				items = items.filter(fullText);
			}
			else if (!string.IsNullOrEmpty(e.Text))
			{
				items = items.filter(e.Text);
			}
			comboBox.ItemsSource = items;
		}

		private static void pasting(ComboBox comboBox, DataObjectPastingEventArgs e)
		{
			comboBox.Tag = comboBox.Tag ?? comboBox.ItemsSource;

			comboBox.IsDropDownOpen = true;

			string pastedText = (string) e.DataObject.GetData(typeof(string));
			string fullText = comboBox.insertFullText(pastedText);

			var items = comboBox.getOriginalItemSource();
			if (!string.IsNullOrEmpty(fullText))
			{
				items = items.filter(fullText);
			}
			comboBox.ItemsSource = items;
		}

		private static void previewKeyUp(ComboBox comboBox, KeyEventArgs e)
		{
			comboBox.Tag = comboBox.Tag ?? comboBox.ItemsSource;

			bool isCut = e.Key == Key.X && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
			bool isKeyDeleting = e.Key == Key.Back || e.Key == Key.Delete || isCut;
			if (isKeyDeleting)
			{
				comboBox.IsDropDownOpen = true;

				var items = comboBox.getOriginalItemSource();
				if (!string.IsNullOrEmpty(comboBox.Text))
				{
					items = items.filter(comboBox.Text);
				}
				comboBox.ItemsSource = items;
			}
		}

		private static IEnumerable<INamed> getOriginalItemSource(this ComboBox comboBox)
		{
			return (comboBox.Tag as System.Collections.IEnumerable ?? Array.Empty<INamed>()).OfType<INamed>();
		}

		private static IEnumerable<INamed> filter(this IEnumerable<INamed> items, string searchPattern)
		{
			return items.Where(p => p.Name.GetValue(_currentLanguage).IndexOf(searchPattern, StringComparison.InvariantCultureIgnoreCase) >= 0);
		}

		private static string insertFullText(this ComboBox comboBox, string searchPattern)
		{
			return comboBox.Text.Insert(getChildOfType<TextBox>(comboBox).CaretIndex, searchPattern);
		}

		private static T getChildOfType<T>(DependencyObject dependencyObject)
			where T : DependencyObject
		{
			if (dependencyObject == null) return null;
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
			{
				var child = VisualTreeHelper.GetChild(dependencyObject, i);
				var result = (child as T) ?? getChildOfType<T>(child);
				if (result != null) return result;
			}
			return null;
		}

		#endregion

		private static ILanguage _currentLanguage
		{ get { return (Application.Current as IInventorApplication).CurrentLanguage; } }
	}
}
