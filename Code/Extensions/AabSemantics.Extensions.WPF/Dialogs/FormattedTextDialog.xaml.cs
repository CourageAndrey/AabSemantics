using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace AabSemantics.Extensions.WPF.Dialogs
{
	public partial class FormattedTextDialog
	{
		/// <summary>Creates a dialog showing structured text with clickable knowledge links.</summary>
		/// <param name="language">Language to render in.</param>
		/// <param name="text">Text to show.</param>
		/// <param name="linkClicked">Called when the user clicks a link to a knowledge item.</param>
		public FormattedTextDialog(ILanguage language, IText text, Action<IKnowledge> linkClicked)
		{
			InitializeComponent();

			_knowledgeById = new Dictionary<string, IKnowledge>();
			foreach (var knowledge in text.GetParameters().Values)
			{
				_knowledgeById[knowledge.ID] = knowledge;
			}

			_linkClicked = linkClicked;

			var browser = (WebBrowser) windowsFormsHost.Child;
			browser.DocumentText = TextRenders.Html.RenderText(text, language).ToString();
			browser.Navigating += browserNavigating;

			textBox.Text = TextRenders.PlainString.RenderText(text, language).ToString();
		}

		private void browserNavigating(object sender, WebBrowserNavigatingEventArgs webBrowserNavigatingEventArgs)
		{
			webBrowserNavigatingEventArgs.Cancel = true;

			IKnowledge knowledge;
			if (_linkClicked != null && _knowledgeById.TryGetValue(webBrowserNavigatingEventArgs.Url.LocalPath, out knowledge))
			{
				_linkClicked(knowledge);
			}
		}

		private readonly IDictionary<string, IKnowledge> _knowledgeById;
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

	/// <summary>Ready-made ways of showing structured text to the user.</summary>
	public static class FormattedTextDialogUseCases
	{
		/// <summary>Shows an answer together with its explanation.</summary>
		/// <param name="answer">Answer to show.</param>
		/// <param name="ownerWindow">Window the dialog belongs to.</param>
		/// <param name="language">Language to render in.</param>
		/// <param name="knowledgeObjectPicked">Called when the user clicks a link to a knowledge item.</param>
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

		/// <summary>
		/// Shows every statement the network holds. Collecting them can take a while on a large
		/// network, so it runs behind a dialog offering to cancel it; nothing is shown when the
		/// user does cancel.
		/// </summary>
		/// <param name="semanticNetwork">Network to describe.</param>
		/// <param name="ownerWindow">Window the dialog belongs to.</param>
		/// <param name="language">Language to render in.</param>
		/// <param name="knowledgeObjectPicked">Called when the user clicks a link to a knowledge item.</param>
		public static void DisplayRulesDescription(this ISemanticNetwork semanticNetwork, Window ownerWindow, ILanguage language, Action<IKnowledge> knowledgeObjectPicked)
		{
			IText rules;
			if (ProcessingDialog.TryRun(
				ownerWindow,
				language,
				language.GetExtension<IWpfUiModule>().Misc.DescribingRules,
				cancellationToken => semanticNetwork.DescribeRulesAsync(cancellationToken),
				out rules))
			{
				new FormattedTextDialog(
					language,
					rules,
					knowledgeObjectPicked)
				{
					Owner = ownerWindow,
					Title = language.GetExtension<IWpfUiModule>().Misc.Rules,
				}.Show();
			}
		}

		/// <summary>
		/// Runs the consistency check and shows its findings. The check walks the whole network,
		/// so it runs behind a dialog offering to cancel it; nothing is shown when the user does
		/// cancel.
		/// </summary>
		/// <param name="semanticNetwork">Network to validate.</param>
		/// <param name="ownerWindow">Window the dialog belongs to.</param>
		/// <param name="language">Language to render in.</param>
		/// <param name="knowledgeObjectPicked">Called when the user clicks a link to a knowledge item.</param>
		public static void DisplayConsistencyCheckResult(this ISemanticNetwork semanticNetwork, Window ownerWindow, ILanguage language, Action<IKnowledge> knowledgeObjectPicked)
		{
			IText checkResult;
			if (ProcessingDialog.TryRun(
				ownerWindow,
				language,
				language.GetExtension<IWpfUiModule>().Misc.CheckingConsistency,
				cancellationToken => semanticNetwork.CheckConsistencyAsync(cancellationToken),
				out checkResult))
			{
				new FormattedTextDialog(
					language,
					checkResult,
					knowledgeObjectPicked)
				{
					Owner = ownerWindow,
					Title = language.Statements.Consistency.CheckResult,
				}.Show();
			}
		}
	}
}
