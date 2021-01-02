using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;

using Inventor.Core;
using Inventor.Core.Localization;

namespace Inventor.Client.UI
{
	public partial class FormattedTextDialog
	{
		public FormattedTextDialog(FormattedText text, Action<INamed> linkClicked)
		{
			InitializeComponent();

			_parameters = text.GetAllParameters();
			_linkClicked = linkClicked;

			var browser = (WebBrowser) windowsFormsHost.Child;
			browser.DocumentText = text.GetHtml().ToString();
			browser.Navigating += browserNavigating;
		}

		private void browserNavigating(object sender, WebBrowserNavigatingEventArgs webBrowserNavigatingEventArgs)
		{
			if (_linkClicked != null)
			{
				_linkClicked(_parameters[webBrowserNavigatingEventArgs.Url.Fragment]);
			}
			webBrowserNavigatingEventArgs.Cancel = true;
		}

		private readonly IDictionary<string, INamed> _parameters;
		private readonly Action<INamed> _linkClicked;

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
