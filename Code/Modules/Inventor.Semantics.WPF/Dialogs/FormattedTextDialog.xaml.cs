using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;

using Inventor.Semantics;

namespace Inventor.WPF.Dialogs
{
	public partial class FormattedTextDialog
	{
		public FormattedTextDialog(ILanguage language, IText text, Action<IKnowledge> linkClicked)
		{
			InitializeComponent();

			_parameters = text.GetParameters();
			_linkClicked = linkClicked;

			var browser = (WebBrowser) windowsFormsHost.Child;
			browser.DocumentText = TextRepresenters.Html.RepresentText(text, language).ToString();
			browser.Navigating += browserNavigating;
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
