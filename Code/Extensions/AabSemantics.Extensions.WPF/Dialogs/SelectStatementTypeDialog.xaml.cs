using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using AabSemantics.Metadata;

namespace AabSemantics.Extensions.WPF.Dialogs
{
	public partial class SelectStatementTypeDialog
	{
		/// <summary>Creates the dialog.</summary>
		public SelectStatementTypeDialog()
		{
			InitializeComponent();
		}

		/// <summary>Fills the type list with the statement types the network's modules provide.</summary>
		/// <param name="language">Language the dialog is localized in.</param>
		/// <param name="semanticNetwork">Network whose modules supply the types.</param>
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

		/// <summary>Statement type the user has chosen.</summary>
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
