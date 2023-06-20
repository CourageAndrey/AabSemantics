using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Inventor.Semantics.Metadata;

namespace Inventor.Semantics.Extensions.WPF.Dialogs
{
	public partial class SelectStatementTypeDialog
	{
		public SelectStatementTypeDialog()
		{
			InitializeComponent();
		}

		public void Initialize(ILanguage language, ISemanticNetwork semanticNetwork)
		{
			_radioGroup.Children.Clear();
			foreach (var statementDefinition in Repositories.Statements.Definitions.Values)
			{
				var radioButton = new RadioButton
				{
					Margin = new Thickness(5),
					Content = statementDefinition.GetName(language),
					Tag = statementDefinition.Type,
				};
				_radioGroup.Children.Add(radioButton);
			}

			Title = language.GetExtension<IWpfUiModule>().Ui.StatementTypeDialogHeader;
			_buttonOk.Content = language.GetExtension<IWpfUiModule>().Common.Ok;
			_buttonCancel.Content = language.GetExtension<IWpfUiModule>().Common.Cancel;
		}

		public Type SelectedType
		{
			get { return _radioGroup.Children.OfType<RadioButton>().FirstOrDefault(radioButton => radioButton.IsChecked == true)?.Tag as Type; }
#pragma warning disable 252,253
			set { _radioGroup.Children.OfType<RadioButton>().First(radioButton => radioButton.Tag == value).IsChecked = true; }
#pragma warning restore 252,253
		}

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
