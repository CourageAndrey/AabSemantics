using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Sef.Interfaces;
using Sef.Localization;

namespace Sef.UI
{
    public static class UiHelper
    {
        #region Complex header

        public static Object ComposeHeader(System.Drawing.Image image, Size imageSize, String text)
        {
            if (image != null)
            {
                var stackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };
                stackPanel.Children.Add(new Image
                {
                    Source = image.ToSource(),
                    Height = imageSize.Height,
                    Width = imageSize.Width
                });
                stackPanel.Children.Add(new TextBlock
                {
                    Text = text,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(10, 0, 0, 0)
                });
                return stackPanel;
            }
            else
            {
                return text;
            }
        }

        public static void ChangeText(this Object header, String newText)
        {
            ((StackPanel) header).Children.OfType<TextBlock>().Single().Text = newText;
        }

        #endregion

        #region Menu from commands

        public static void UpdateFromCommand(this MenuItem menuItem, ICommand command)
        {
            if (command is ILocalizable)
            {
                (command as ILocalizable).Localize();
            }
            menuItem.Command = command;
            if (command is IHasImage)
            {
                menuItem.Icon = new Image { Width = 16, Height = 16, Source = (command as IHasImage).Image.ToSource() };
            }
        }

        public static MenuItem ConvertToMenuItem(this ICommand command)
        {
            var menuItem = new MenuItem();
            menuItem.UpdateFromCommand(command);
            return menuItem;
        }

        #endregion

        public static void UpdateCommandsEnabled()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public static void ProcessValidationError(ValidationErrorEventArgs e)
        {
            var control = e.OriginalSource as Control;
            if (control != null)
            {
                control.ToolTip = e.Action == ValidationErrorEventAction.Added
                    ? e.Error.ErrorContent
                    : null;
            }
        }
    }
}
