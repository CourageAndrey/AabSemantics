using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;

using Inventor.Core;

namespace Inventor.Client.Dialogs
{
	public partial class FormattedTextDialog
	{
		public FormattedTextDialog(ILanguage language, Core.Text.TextContainer text, Action<IKnowledge> linkClicked)
		{
			InitializeComponent();

			_parameters = text.GetAllParameters();
			_linkClicked = linkClicked;

			var browser = (WebBrowser) windowsFormsHost.Child;
			browser.DocumentText = text.GetHtml(language).ToString();
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
}
