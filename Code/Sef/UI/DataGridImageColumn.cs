using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

using Sef.Common;

namespace Sef.UI
{
    public class DataGridImageColumn : DataGridBoundColumn
    {
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, Object dataItem)
        {
            return null;
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, Object dataItem)
        {
            var pictureBox = new Image();
            
            var binding = Binding as Binding;
            dataItem = binding != null
                ? dataItem.GetPropertyValue(binding.Path.Path)
                : null;

            if (dataItem is ImageSource)
            {
                pictureBox.Source = dataItem as ImageSource;
            }
            else if (dataItem is System.Drawing.Image)
            {
                pictureBox.Source = (dataItem as System.Drawing.Image).ToSource();
            }
            return pictureBox;
        }
    }
}
