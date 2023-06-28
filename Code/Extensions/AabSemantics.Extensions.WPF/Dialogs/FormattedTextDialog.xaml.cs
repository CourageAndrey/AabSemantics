using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace AabSemantics.Extensions.WPF.Dialogs
{
	public partial class FormattedTextDialog
	{
		public FormattedTextDialog(ILanguage language, IText text, Action<IKnowledge> linkClicked)
		{
			InitializeComponent();

			_parameters = text.GetParameters();
			_linkClicked = linkClicked;

			var browser = (WebBrowser) windowsFormsHost.Child;
			browser.DocumentText = TextRenders.Html.RenderText(text, language).ToString();
			browser.Navigating += browserNavigating;

			textBox.Text = TextRenders.PlainString.RenderText(text, language).ToString();
		}

		private void browserNavigating(object sender, WebBrowserNavigatingEventArgs webBrowserNavigatingEventArgs)
		{
			if (_linkClicked != null)
			{
				string key = webBrowserNavigatingEventArgs.Url.LocalPath;
				_linkClicked(_parameters[key]);
			}
			webBrowserNavigatingEventArgs.Cancel = true;
		}

		private readonly IDictionary<string, IKnowledge> _parameters;
		private readonly Action<IKnowledge> _linkClicked;

		private void dialogLoaded(object sender, RoutedEventArgs e)
		{
			if (Owner != null)
			{
				Left = Owner.Left + Owner.Width/2;
				Top = Owner.Top;
			}
		}

		private void saveClick(object sender, RoutedEventArgs e)
		{
			string defaultExt = null;
			string fileFilter = null;
			string content = null;

			if (tabControl.SelectedItem == tabText)
			{
				defaultExt = ".txt";
				fileFilter = "TXT|*.txt";

				content = textBox.Text;
			}
			else if (tabControl.SelectedItem == tabHtml)
			{
				defaultExt = ".html";
				fileFilter = "HTML|*.html";

				var browser = (WebBrowser) windowsFormsHost.Child;
				content = browser.DocumentText;
			}

			var dialog = new Microsoft.Win32.SaveFileDialog
			{
				DefaultExt = defaultExt,
				Filter = fileFilter,
				RestoreDirectory = true,
			};

			if (dialog.ShowDialog() == true)
			{
				File.WriteAllText(dialog.FileName, content);
			}
		}
	}

	public static class FormattedTextDialogUseCases
	{
		public static void Display(this IAnswer answer, Window ownerWindow, ILanguage language, Action<IKnowledge> knowledgeObjectPicked)
		{
			new FormattedTextDialog(
				language,
				answer.GetDescriptionWithExplanation(),
				knowledgeObjectPicked)
			{
				Owner = ownerWindow,
				Title = language.GetExtension<IWpfUiModule>().Misc.Answer,
			}.Show();
		}

		public static void DisplayRulesDescription(this ISemanticNetwork semanticNetwork, Window ownerWindow, ILanguage language, Action<IKnowledge> knowledgeObjectPicked)
		{
			new FormattedTextDialog(
				language,
				semanticNetwork.DescribeRules(),
				knowledgeObjectPicked)
			{
				Owner = ownerWindow,
				Title = language.GetExtension<IWpfUiModule>().Misc.Rules,
			}.Show();
		}

		public static void DisplayConsistencyCheckResult(this ISemanticNetwork semanticNetwork, Window ownerWindow, ILanguage language, Action<IKnowledge> knowledgeObjectPicked)
		{
			new FormattedTextDialog(
				language,
				semanticNetwork.CheckConsistency(),
				knowledgeObjectPicked)
			{
				Owner = ownerWindow,
				Title = language.Statements.Consistency.CheckResult,
			}.Show();
		}
	}
}
