using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

using Inventor.Core;

namespace Inventor.Client.UI
{
	public partial class QuestionDialog
	{
		#region Properties

		public Question Question
		{ get; private set; }

		#endregion

		public QuestionDialog(KnowledgeBase knowledgeBase)
		{
			InitializeComponent();

			((NamedConverter) Resources["namedConverter"]).Language = Core.Localization.LanguageEx.CurrentEx;

			Title = Core.Localization.LanguageEx.CurrentEx.Ui.QuestionDialog.Title;

			panelSelectQuestion.DataContext = Question = null;
			panelSelectQuestion.Visibility = Visibility.Visible;
			panelQuestionParams.Visibility = Visibility.Hidden;

			this._knowledgeBase = knowledgeBase;
			foreach (var questionDraft in Question.AllSupportedTypes)
			{
				_questions[questionDraft.Key()] = questionDraft.Value;
			}
			comboBoxQuestion.ItemsSource = _questions.Keys.OrderBy(q => q);

			_propertySelectorCreators = new Dictionary<Type, CreatePropertySelectorDelegate>
			{
				{ typeof (Concept), createConceptSelector },
				{ typeof (bool), createCheckBox },
			};
		}

		private readonly Dictionary<string, Func<Question>> _questions = new Dictionary<string, Func<Question>>();
		private readonly KnowledgeBase _knowledgeBase;
		private readonly List<ComboBox> _requiredFieldSelectors = new List<ComboBox>();

		private void buttonCreateClick(object sender, RoutedEventArgs e)
		{
			panelSelectQuestion.Visibility = Visibility.Collapsed;
			panelQuestionParams.Visibility = Visibility.Visible;
			Question = _questions[comboBoxQuestion.Text]();
			textBoxQuestion.Text = comboBoxQuestion.Text;

			int gridRow = 1;
			foreach (var property in Question.GetType().GetProperties())
			{
				var propertyDescriptor = property.GetCustomAttribute<PropertyDescriptorAttribute>();
				if (propertyDescriptor != null)
				{
					panelQuestionParams.RowDefinitions.Insert(gridRow, new RowDefinition { Height = GridLength.Auto });
					_propertySelectorCreators[property.PropertyType](propertyDescriptor, property, gridRow);
					gridRow++;
				}
			}
			buttonOk.IsEnabled = Question != null && _requiredFieldSelectors.TrueForAll(cb => cb.SelectedValue != null);
		}

		#region Property selectors

		private delegate void CreatePropertySelectorDelegate(PropertyDescriptorAttribute propertyDescriptor, PropertyInfo propertyInfo, int gridRow);
		private readonly Dictionary<Type, CreatePropertySelectorDelegate> _propertySelectorCreators;

		private void createConceptSelector(PropertyDescriptorAttribute propertyDescriptor, PropertyInfo propertyInfo, int gridRow)
		{
			TextBlock textLabel;
			panelQuestionParams.Children.Add(textLabel = new TextBlock
			{
				Margin = new Thickness(2),
				DataContext = Core.Localization.LanguageEx.CurrentEx,
			});
			textLabel.SetBinding(TextBlock.TextProperty, new Binding
			{
				Path = new PropertyPath(propertyDescriptor.NamePath),
				Mode = BindingMode.OneWay,
				Converter = new FormatConverter { Required = propertyDescriptor.Required },
			});
			textLabel.SetValue(Grid.RowProperty, gridRow);
			textLabel.SetValue(Grid.ColumnProperty, 0);

			ComboBox comboBox;
			panelQuestionParams.Children.Add(comboBox = new ComboBox
			{
				ItemsSource = _knowledgeBase.Concepts,
				Margin = new Thickness(2),
				MinWidth = 50,
				DataContext = Question,
			});
			comboBox.SetValue(Grid.RowProperty, gridRow);
			comboBox.SetValue(Grid.ColumnProperty, 1);
			if (propertyDescriptor.Required)
			{
				_requiredFieldSelectors.Add(comboBox);
				comboBox.SelectionChanged += propertyValueSelected;
			}
			comboBox.SetBinding(Selector.SelectedItemProperty, new Binding {Path = new PropertyPath(propertyInfo.Name), Mode = BindingMode.TwoWay});
		}

		private void createCheckBox(PropertyDescriptorAttribute propertyDescriptor, PropertyInfo propertyInfo, int gridRow)
		{
			CheckBox checkBox;
			panelQuestionParams.Children.Add(checkBox = new CheckBox
			{
				Margin = new Thickness(2),
				DataContext = Question,
			});
			checkBox.SetBinding(ToggleButton.IsCheckedProperty, new Binding
			{
				Path = new PropertyPath(propertyInfo.Name),
				Mode = BindingMode.TwoWay,
			});
			checkBox.SetValue(Grid.RowProperty, gridRow);
			checkBox.SetValue(Grid.ColumnProperty, 0);
			checkBox.SetValue(Grid.ColumnSpanProperty, 2);

			var textLabel = new TextBlock
			{
				Margin = new Thickness(0, 0, 0, 2),
				DataContext = Core.Localization.LanguageEx.CurrentEx,
			};
			textLabel.SetBinding(TextBlock.TextProperty, new Binding
			{
				Path = new PropertyPath(propertyDescriptor.NamePath),
				Mode = BindingMode.OneWay,
				Converter = new FormatConverter { Required = propertyDescriptor.Required },
			});
			checkBox.Content = textLabel;
		}

		#endregion

		private void propertyValueSelected(object sender, SelectionChangedEventArgs e)
		{
			buttonOk.IsEnabled = Question != null && _requiredFieldSelectors.TrueForAll(cb => cb.SelectedValue != null);
		}

		private void questionTypeSelected(object sender, SelectionChangedEventArgs e)
		{
			buttonCreateQuestion.IsEnabled = comboBoxQuestion.SelectedItem != null;
		}

		private void buttonOk_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void clearQuestion_Click(object sender, RoutedEventArgs e)
		{
			panelSelectQuestion.Visibility = Visibility.Visible;
			while (panelQuestionParams.Children.Count > 2)
			{
				panelQuestionParams.Children.RemoveAt(panelQuestionParams.Children.Count - 1);
			}
			while (panelQuestionParams.RowDefinitions.Count > 2)
			{
				panelQuestionParams.RowDefinitions.RemoveAt(panelQuestionParams.RowDefinitions.Count - 2);
			}
			panelQuestionParams.Visibility = Visibility.Collapsed;
			Question = null;
			comboBoxQuestion.SelectedItem = null;
			_requiredFieldSelectors.Clear();
		}

		private class FormatConverter : IValueConverter
		{
			public bool Required;

			public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
			{
				return string.Format("{0}{1} :", (string) value, Required
					? string.Format(" ({0})", Core.Localization.LanguageEx.CurrentEx.Misc.Required)
					: string.Empty);
			}

			public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
			{
				throw new NotSupportedException();
			}
		}
	}
}
