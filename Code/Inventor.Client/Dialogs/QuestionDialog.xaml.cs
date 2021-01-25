using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

using Inventor.Client.Converters;
using Inventor.Client.ViewModels.Questions;
using Inventor.Core;

namespace Inventor.Client.Dialogs
{
	public partial class QuestionDialog
	{
		#region Properties

		public IQuestionViewModel Question
		{ get; private set; }

		private readonly ILanguage _language;

		#endregion

		public QuestionDialog(IKnowledgeBase knowledgeBase, ILanguage language)
		{
			_language = language;

			InitializeComponent();

			var localizationProvider = (ObjectDataProvider) Resources["language"];
			localizationProvider.ConstructorParameters.Add(_language);
			((NamedConverter) Resources["namedConverter"]).Language = _language;

			Title = _language.Ui.QuestionDialog.Title;

			panelSelectQuestion.DataContext = Question = null;
			panelSelectQuestion.Visibility = Visibility.Visible;
			panelQuestionParams.Visibility = Visibility.Hidden;

			_knowledgeBase = knowledgeBase;
			foreach (var questionDefinition in knowledgeBase.Context.QuestionRepository.QuestionDefinitions.Values)
			{
				var genericType = typeof(QuestionViewModel<>).MakeGenericType(questionDefinition.QuestionType);
				var viewModelType = Assembly.GetExecutingAssembly().GetTypes().First(t => !t.IsAbstract && genericType.IsAssignableFrom(t));
				_questions[questionDefinition.GetName(_language)] = () => Activator.CreateInstance(viewModelType) as IQuestionViewModel;
			}
			comboBoxQuestion.ItemsSource = _questions.Keys.OrderBy(q => q);

			_propertySelectorCreators = new Dictionary<Type, CreatePropertySelectorDelegate>
			{
				{ typeof(IConcept), createConceptSelector },
				{ typeof(bool), createCheckBox },
				{ typeof(IStatement), createStatementEditor },
				{ typeof(ICollection<IStatement>), createStatementsList },
				{ typeof(IQuestionViewModel), createQuestionEditor },
			};
		}

		private readonly Dictionary<string, Func<IQuestionViewModel>> _questions = new Dictionary<string, Func<IQuestionViewModel>>();
		private readonly IKnowledgeBase _knowledgeBase;
		private readonly List<ComboBox> _requiredFieldSelectors = new List<ComboBox>();

		private void buttonCreateClick(object sender, RoutedEventArgs e)
		{
			beginEditQuestion(_questions[comboBoxQuestion.Text]());
		}

		private void beginEditQuestion(IQuestionViewModel question)
		{
			panelSelectQuestion.Visibility = Visibility.Collapsed;
			panelQuestionParams.Visibility = Visibility.Visible;
			Question = question;
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
				DataContext = _language,
			});
			textLabel.SetBinding(TextBlock.TextProperty, new Binding
			{
				Path = new PropertyPath(propertyDescriptor.NamePath),
				Mode = BindingMode.OneWay,
				Converter = new FormatConverter(propertyDescriptor.Required, _language),
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
				DataContext = _language,
			};
			textLabel.SetBinding(TextBlock.TextProperty, new Binding
			{
				Path = new PropertyPath(propertyDescriptor.NamePath),
				Mode = BindingMode.OneWay,
				Converter = new FormatConverter(propertyDescriptor.Required, _language),
			});
			checkBox.Content = textLabel;
		}

		private void createStatementEditor(PropertyDescriptorAttribute propertyDescriptor, PropertyInfo propertyInfo, int gridRow)
		{
			Button editButton;
			panelQuestionParams.Children.Add(editButton = new Button
			{
				Margin = new Thickness(2),
				Content = $"{_language.QuestionNames.ParamStatement}: ...",
				DataContext = Question,
			});
			editButton.SetBinding(FrameworkElement.TagProperty, new Binding
			{
				Path = new PropertyPath(propertyInfo.Name),
				Mode = BindingMode.TwoWay,
			});
			editButton.SetValue(Grid.RowProperty, gridRow);
			editButton.SetValue(Grid.ColumnProperty, 0);
			editButton.SetValue(Grid.ColumnSpanProperty, 2);
			editButton.Click += (sender, args) =>
			{
				StatementViewModel viewModel;

				var statement = editButton.Tag as IStatement;
				if (statement == null || MessageBox.Show(_language.Ui.CreateNewStatement, _language.Common.Question, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
				{
					Type statementType = null;
					var statementTypesDialog = new SelectStatementTypeDialog
					{
						Owner = this,
					};
					statementTypesDialog.Initialize(_language);
					if (statementTypesDialog.ShowDialog() == true)
					{
						statementType = statementTypesDialog.SelectedType;
					}
					if (statementType == null) return;

					viewModel = (StatementViewModel) ViewModels.Factory.CreateByCoreType(statementType);
				}
				else
				{
					viewModel = ViewModels.Factory.CreateStatementByInstance(statement, _language);
				}

				var editDialog = viewModel.CreateEditDialog(this, _knowledgeBase, _language);

				if (editDialog.ShowDialog() == true)
				{
					statement = viewModel.CreateStatement();
					editButton.Tag = statement;
					editButton.Content = $"{_language.QuestionNames.ParamStatement}: {statement.DescribeTrue(_language).GetPlainText(_language)}";
				}
			};
		}

		private void createStatementsList(PropertyDescriptorAttribute propertyDescriptor, PropertyInfo propertyInfo, int gridRow)
		{
			GroupBox groupBox;
			panelQuestionParams.Children.Add(groupBox = new GroupBox
			{
				Header = _language.QuestionNames.ParamConditions + ":",
				Margin = new Thickness(2),
			});
			groupBox.SetValue(Grid.RowProperty, gridRow);
			groupBox.SetValue(Grid.ColumnProperty, 0);
			groupBox.SetValue(Grid.ColumnSpanProperty, 2);

			var listBox = new ListBox
			{
				DataContext = Question,
			};
			listBox.SetBinding(ItemsControl.ItemsSourceProperty, new Binding
			{
				Path = new PropertyPath(propertyInfo.Name),
				Mode = BindingMode.OneWay,
#warning Display all statements correctly.
			});

			var addButton = new Button
			{
				Content = "+",
				Margin = new Thickness(2),
				Padding = new Thickness(5, 2, 5, 2),
			};
			addButton.Click += (sender, args) =>
			{
				StatementViewModel viewModel;

				Type statementType = null;
				var statementTypesDialog = new SelectStatementTypeDialog
				{
					Owner = this,
				};
				statementTypesDialog.Initialize(_language);
				if (statementTypesDialog.ShowDialog() == true)
				{
					statementType = statementTypesDialog.SelectedType;
				}
				if (statementType == null) return;

				viewModel = (StatementViewModel) ViewModels.Factory.CreateByCoreType(statementType);

				var editDialog = viewModel.CreateEditDialog(this, _knowledgeBase, _language);

				if (editDialog.ShowDialog() == true)
				{
					var statement = viewModel.CreateStatement();
					((ICollection<IStatement>) listBox.ItemsSource).Add(statement);
				}
			};

			var deleteButton = new Button
			{
				Content = "-",
				Margin = new Thickness(2),
				Padding = new Thickness(5, 2, 5, 2),
			};
			deleteButton.Click += (sender, args) =>
			{
				var statements = listBox.SelectedItems.OfType<IStatement>().ToList();
				foreach (var statement in statements)
				{
					listBox.Items.Remove(statement);
				}
			};

			var editButton = new Button
			{
				Content = "...",
				Margin = new Thickness(2),
				Padding = new Thickness(5, 2, 5, 2),
			};
			editButton.Click += (sender, args) =>
			{
				var statement = listBox.SelectedItem as IStatement;
				if (statement == null) return;

				var viewModel = ViewModels.Factory.CreateStatementByInstance(statement, _language);

				var editDialog = viewModel.CreateEditDialog(this, _knowledgeBase, _language);

				if (editDialog.ShowDialog() == true)
				{
					viewModel.ApplyUpdate();
#warning Ensure that corresponding ListBox item is also updated.
				}
			};

			var stackPanel = new StackPanel
			{
				Orientation = Orientation.Vertical,
			};
			stackPanel.SetValue(DockPanel.DockProperty, Dock.Right);
			stackPanel.Children.Add(addButton);
			stackPanel.Children.Add(deleteButton);
			stackPanel.Children.Add(editButton);

			var dockPanel = new DockPanel
			{
				LastChildFill = true,
			};

			dockPanel.Children.Add(stackPanel);
			dockPanel.Children.Add(listBox);

			groupBox.Content = dockPanel;
		}

		private void createQuestionEditor(PropertyDescriptorAttribute propertyDescriptor, PropertyInfo propertyInfo, int gridRow)
		{
			Button editButton;
			panelQuestionParams.Children.Add(editButton = new Button
			{
				Margin = new Thickness(2),
				Content = $"{_language.QuestionNames.ParamQuestion}: ...",
				DataContext = Question,
			});
			editButton.SetBinding(FrameworkElement.TagProperty, new Binding
			{
				Path = new PropertyPath(propertyInfo.Name),
				Mode = BindingMode.TwoWay,
			});
			editButton.SetValue(Grid.RowProperty, gridRow);
			editButton.SetValue(Grid.ColumnProperty, 0);
			editButton.SetValue(Grid.ColumnSpanProperty, 2);
			editButton.Click += (sender, args) =>
			{
				var question = editButton.Tag as IQuestionViewModel;
				if (question == null || MessageBox.Show(_language.Ui.CreateNewQuestion, _language.Common.Question, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
				{
					var dialog = new QuestionDialog(_knowledgeBase, _language)
					{
						Owner = this,
					};
					if (dialog.ShowDialog() == true)
					{
						question = dialog.Question;
					}
				}
				else
				{
					var dialog = new QuestionDialog(_knowledgeBase, _language)
					{
						Owner = this,
					};
					dialog.beginEditQuestion(question);
					if (dialog.ShowDialog() != true)
					{
						question = null;
					}
				}

				if (question != null)
				{
					editButton.Tag = question;
					var questionDefinition = _knowledgeBase.Context.QuestionRepository.QuestionDefinitions[question.GetType()];
					editButton.Content = $"{_language.QuestionNames.ParamQuestion}: {questionDefinition.GetName(_language)}";
#warning Display full form of questions instead of raw stubs without data.
				}
			};
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
			private readonly bool _required;
			private readonly ILanguage _language;

			public FormatConverter(bool required, ILanguage language)
			{
				_required = required;
				_language = language;
			}

			public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
			{
				return string.Format("{0}{1} :", (string) value, _required
					? string.Format(" ({0})", _language.Misc.Required)
					: string.Empty);
			}

			public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
			{
				throw new NotSupportedException();
			}
		}
	}
}
