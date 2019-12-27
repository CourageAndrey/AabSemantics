using System;
using System.Windows;
using System.Windows.Controls;

using Sef.Interfaces;

namespace Sef.UI
{
    public class EditorBase : UserControl, IEditor
    {
        #region Implementation of IEditor

        public Object EditValue
        {
            get { return GetValue(EditValueProperty); }
            set { SetValue(EditValueProperty, value); }
        }

        public Boolean ReadOnly
        {
            get { return (Boolean) GetValue(ReadOnlyProperty); }
            set { SetValue(ReadOnlyProperty, value); }
        }

        public event EventHandler EditValueChanged;

        public event EventHandler EditValueCommited;

        public Control AsControl
        { get { return this; } }

        #endregion

        #region Dependency properties

        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register(
            "ReadOnly",
            typeof(Boolean),
            typeof(EditorBase),
            new FrameworkPropertyMetadata(
                ReadOnlyDefault,
                FrameworkPropertyMetadataOptions.AffectsRender,
                (d, e) => ((EditorBase) d).SetReadOnly((Boolean) e.NewValue)));

        public static readonly DependencyProperty EditValueProperty = DependencyProperty.Register(
            "EditValue",
            typeof(Object),
            typeof(EditorBase),
            new FrameworkPropertyMetadata(
                EditValueDefault,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                (d, e) => ((EditorBase) d).SetEditValue(e.NewValue),
                (d, o) => ((EditorBase) d).CoerceEditValue(o)));

        public const Boolean ReadOnlyDefault = false;
        public const Object EditValueDefault = null;

        #endregion

        #region Protected interface

        protected virtual void SetEditValue(Object value)
        { }

        protected virtual void SetReadOnly(Boolean readOnly)
        { }

        protected virtual Object CoerceEditValue(Object value)
        {
            return value;
        }

        protected void RaiseValueChanged(Boolean commited)
        {
            if (commited)
            {
                var handlerCommited = EditValueCommited;
                if (handlerCommited != null)
                {
                    handlerCommited(this, EventArgs.Empty);
                }
            }

            var handlerChanged = EditValueChanged;
            if (handlerChanged != null)
            {
                handlerChanged(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}
