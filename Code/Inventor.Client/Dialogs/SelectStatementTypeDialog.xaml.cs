using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Inventor.Core;

namespace Inventor.Client.Dialogs
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
			foreach (var statementDefinition in Repositories.Statements.StatementDefinitions.Values)
			{
				var radioButton = new RadioButton
				{
					Margin = new Thickness(5),
					Content = statementDefinition.GetName(language),
					Tag = statementDefinition.StatementType,
				};
				_radioGroup.Children.Add(radioButton);
			}

			Title = language.Ui.StatementTypeDialogHeader;
			_buttonOk.Content = language.Common.Ok;
			_buttonCancel.Content = language.Common.Cancel;
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
