using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

using Sef.Common;
using Sef.Localization;

namespace Sef.UI
{
    public class DataGridImagePopupColumn : DataGridBoundColumn
    {
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, Object dataItem)
        {
            return null;
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, Object dataItem)
        {
            if (String.IsNullOrEmpty(ImageCheckPath) || (Boolean) dataItem.GetPropertyValue(ImageCheckPath))
            {
                var button = new Button
                {
                    Content = "...",
                    ToolTip = Language.Current.Common.ViewPicture,
                    Margin = new Thickness(1)
                };
                button.Click += (sender, args) =>
                {
                    var binding = Binding as Binding;
                    var picture = binding != null
                        ? dataItem.GetPropertyValue(binding.Path.Path)
                        : null;

                    ImageSource imageSource = null;
                    if (picture is ImageSource)
                    {
                        imageSource = picture as ImageSource;
                    }
                    else if (picture is System.Drawing.Image)
                    {
                        imageSource = (picture as System.Drawing.Image).ToSource();
                    }
                    if (imageSource != null)
                    {
                        new Popup
                        {
                            StaysOpen = false,
                            Placement = PlacementMode.Mouse,
                            MaxWidth = 800,
                            MaxHeight = 600,
                            Child = new Border
                            {
                                BorderBrush = Brushes.Black,
                                BorderThickness = new Thickness(1),
                                Background = Brushes.White,
                                Child = new Image
                                {
                                    Source = imageSource,
                                    Width = imageSource.Width,
                                    Height = imageSource.Height
                                },
                            },
                            IsOpen = true
                        };
                    }
                };
                return button;
            }
            else
            {
                return new Label();
            }
        }

        public String ImageCheckPath
        { get; set; }
    }
}
