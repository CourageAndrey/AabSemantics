using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

using Sef.Interfaces;
using Sef.Localization;

namespace Sef.UI
{
    public partial class CommonDialog : ILocalizable
    {
        private CommonDialog()
        {
            InitializeComponent();
        }

        #region Localization

        public void Localize()
        {
            ((ObjectDataProvider) Resources["language"]).Refresh();
            updateTitle();
            if (control is ILocalizable)
            {
                (control as ILocalizable).Localize();
            }
        }

        private Func<String> title;
        private Control control;

        private void updateTitle()
        {
            Title = title();
        }

        #endregion

        #region Public interface

        public static Boolean? ShowControlDialog(
            Control displayControl,
            Func<String> titleGetter,
            ImageSource icon = null,
            Boolean showInTaskbar = true,
            ResizeMode resizeMode = ResizeMode.NoResize)
        {
            var dialog = new CommonDialog();
            dialog.dockPanel.Children.Add(displayControl);
            displayControl.SetValue(MarginProperty, new Thickness(7));
            dialog.dockPanel.LastChildFill = true;
            dialog.title = titleGetter;
            dialog.control = displayControl;
            dialog.updateTitle();

            dialog.Icon = icon;
            dialog.ShowInTaskbar = showInTaskbar;
            dialog.ResizeMode = resizeMode;

            dialog.Localize();

            if (displayControl is IComplexFocus)
            {
                (displayControl as IComplexFocus).SetRigthFocus();
            }

            var editor = displayControl as IEditor;
            if (editor != null && editor.ReadOnly)
            {
                dialog.buttonOk.Visibility = Visibility.Collapsed;
            }

            dialog.Closed += (sender, args) => dialog.dockPanel.Children.Remove(displayControl);

            return dialog.ShowDialog();
        }

        public static Boolean? ShowControlDialog<ControlT>(
            Func<String> titleGetter,
            ImageSource icon = null,
            Boolean showInTaskbar = true,
            ResizeMode resizeMode = ResizeMode.NoResize)
            where ControlT : Control, new()
        {
            return ShowControlDialog(new ControlT(), titleGetter, icon, showInTaskbar, resizeMode);
        }

        public static Boolean? ShowControlDialog(
            Control displayControl,
            String title,
            ImageSource icon = null,
            Boolean showInTaskbar = true,
            ResizeMode resizeMode = ResizeMode.NoResize)
        {
            return ShowControlDialog(displayControl, () => title, icon, showInTaskbar, resizeMode);
        }

        public static Boolean? ShowControlDialog<ControlT>(
            String title,
            ImageSource icon = null,
            Boolean showInTaskbar = true,
            ResizeMode resizeMode = ResizeMode.NoResize)
            where ControlT : Control, new()
        {
            return ShowControlDialog<ControlT>(() => title, icon, showInTaskbar, resizeMode);
        }

        #endregion

        #region Validation

        private void buttonOk_Click(Object sender, RoutedEventArgs e)
        {
            if (!(control is ISupportsValidation) || (control as ISupportsValidation).Validate())
            {
                DialogResult = true;
            }
        }

        #endregion
    }
}
