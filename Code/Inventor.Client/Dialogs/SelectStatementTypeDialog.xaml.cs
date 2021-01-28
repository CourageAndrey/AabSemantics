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

		public void Initialize(ILanguage language)
		{
			_radioGroup.Children.Clear();
			foreach (var statementDefinition in new[]
			{
				new KeyValuePair<string, Type>(language.StatementNames.Composition, typeof(Core.Statements.HasPartStatement)),
				new KeyValuePair<string, Type>(language.StatementNames.SubjectArea, typeof(Core.Statements.GroupStatement)),
				new KeyValuePair<string, Type>(language.StatementNames.HasSign, typeof(Core.Statements.HasSignStatement)),
				new KeyValuePair<string, Type>(language.StatementNames.Clasification, typeof(Core.Statements.IsStatement)),
				new KeyValuePair<string, Type>(language.StatementNames.SignValue, typeof(Core.Statements.SignValueStatement)),
			})
			{
				var radioButton = new RadioButton
				{
					Margin = new Thickness(5),
					Content = statementDefinition.Key,
					Tag = statementDefinition.Value,
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
