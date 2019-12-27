using System;
using System.Windows;
using System.Windows.Controls;

using Sef.Common;
using Sef.Interfaces;

namespace Sef.UI
{
    public partial class SpinEdit : IEditor<Double>
    {
        public SpinEdit()
        {
            InitializeComponent();
            updateEditorState();
            updateDisplayValue();
        }

        #region Child control handlers

        private void upClick(Object sender, RoutedEventArgs e)
        {
            var newValue = innerValue;
            switch (incrementType)
            {
                case SpinIncrementType.Increase:
                    newValue += incrementValue;
                    break;
                case SpinIncrementType.Multiply:
                    newValue *= incrementValue;
                    break;
                default:
                    throw new NotImplementedException();
            }
            setDirectValue(newValue, true);
        }

        private void downClick(Object sender, RoutedEventArgs e)
        {
            var newValue = innerValue;
            switch (incrementType)
            {
                case SpinIncrementType.Increase:
                    newValue -= incrementValue;
                    break;
                case SpinIncrementType.Multiply:
                    newValue /= incrementValue;
                    break;
                default:
                    throw new NotImplementedException();
            }
            setDirectValue(newValue, true);
        }

        private void textChanged(Object sender, TextChangedEventArgs e)
        {
            if (!suppressTextChanged)
            {
                setDirectValue(Double.Parse(textBox.Text), true);
            }
        }

        private bool suppressTextChanged;

        private void updateEditorState()
        {
            if (innerReadOnly)
            {
                textBox.IsReadOnly = true;
                buttonDown.IsEnabled = buttonUp.IsEnabled = false;
            }
            else
            {
                textBox.IsReadOnly = !textEditingEnabled;
                buttonDown.IsEnabled = buttonUp.IsEnabled = true;
            }
        }

        private void setDirectValue(Double value, bool commit)
        {
            value = Math.Max(minValue, Math.Min(maxValue, value));
            if (Math.Abs(innerValue - value) > Epsilon)
            {
                innerValue = value;
                updateDisplayValue();
                RaiseValueChanged(commit);
            }
        }

        private void updateDisplayValue()
        {
            suppressTextChanged = true;
            textBox.Text = innerValue.ToString("N" + decimalDigits);
            suppressTextChanged = false;
        }

        #endregion

        public enum SpinIncrementType
        {
            Increase,
            Multiply
        }

        #region Properties

        public Double EditValueEx
        {
            get { return (Double) GetValue(EditValueExProperty); }
            set { SetValue(EditValueExProperty, value); }
        }

        public Double MinValue
        {
            get { return (Double) GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public Double MaxValue
        {
            get { return (Double) GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public Double IncrementValue
        {
            get { return (Double) GetValue(IncrementValueProperty); }
            set { SetValue(IncrementValueProperty, value); }
        }

        public SpinIncrementType IncrementType
        {
            get { return (SpinIncrementType) GetValue(IncrementTypeProperty); }
            set { SetValue(IncrementTypeProperty, value); }
        }

        public Byte DecimalDigits
        {
            get { return (Byte) GetValue(DecimalDigitsProperty); }
            set { SetValue(DecimalDigitsProperty, value); }
        }

        public Boolean TextEditingEnabled
        {
            get { return (Boolean) GetValue(TextEditingEnabledProperty); }
            set { SetValue(TextEditingEnabledProperty, value); }
        }

        #endregion

        #region Dependency properties

        #region Properties

        public static readonly DependencyProperty IncrementValueProperty = DependencyProperty.Register(
            "IncrementValue",
            typeof(Double),
            typeof(SpinEdit),
            new FrameworkPropertyMetadata(
                IncrementValueDefault,
                incrementValueChanged));

        public static readonly DependencyProperty IncrementTypeProperty = DependencyProperty.Register(
            "IncrementType",
            typeof(SpinIncrementType),
            typeof(SpinEdit),
            new FrameworkPropertyMetadata(
                IncrementTypeDefault,
                incrementTypeChanged));

        public static readonly DependencyProperty EditValueExProperty = DependencyProperty.Register(
            "EditValueEx",
            typeof(Double),
            typeof(SpinEdit),
            new FrameworkPropertyMetadata(
                ValueDefault,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                valueChanged,
                constrainToRange),
            isValidDoubleValue);

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue",
            typeof(Double),
            typeof(SpinEdit),
            new FrameworkPropertyMetadata(
                MinValueDefault,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                minValueChanged),
            isValidDoubleValue);

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue",
            typeof(Double),
            typeof(SpinEdit),
            new FrameworkPropertyMetadata(
                MaxValueDefault,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                maxValueChanged,
                coerceMaximum),
            isValidDoubleValue);

        public static readonly DependencyProperty DecimalDigitsProperty = DependencyProperty.Register(
            "DecimalDigits",
            typeof(Byte),
            typeof(SpinEdit),
            new FrameworkPropertyMetadata(
                DecimalDigitsDefault,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                decimalDigitsChanged),
            isValidByteValue);

        public static readonly DependencyProperty TextEditingEnabledProperty = DependencyProperty.Register(
            "TextEditingEnabled",
            typeof(Boolean),
            typeof(SpinEdit),
            new FrameworkPropertyMetadata(
                TextEditingEnabledDefault,
                FrameworkPropertyMetadataOptions.AffectsRender,
                textEditingEnabledChanged));

        #endregion

        #region Defaluts

        public const Boolean TextEditingEnabledDefault = true;
        public const SpinIncrementType IncrementTypeDefault = SpinIncrementType.Increase;
        public const Double IncrementValueDefault = 1, MinValueDefault = 0, MaxValueDefault = 100, ValueDefault = MinValueDefault;
        public const Byte DecimalDigitsDefault = 0, DecimalDigitsMin = 0, DecimalDigitsMax = 14;
        public const Double Epsilon = 0.00000000000001;

        #endregion

        #region Direct values

        private Boolean textEditingEnabled = TextEditingEnabledDefault;
        private SpinIncrementType incrementType = IncrementTypeDefault;
        private Double incrementValue = IncrementValueDefault,
                       minValue = MinValueDefault,
                       maxValue = MaxValueDefault;
        private Double innerValue;
        private Byte decimalDigits = DecimalDigitsDefault;
        private Boolean innerReadOnly = ReadOnlyDefault;

        #endregion

        #region Handlers

        private static void incrementValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var spinEdit = (SpinEdit) d;
            var newIncrement = (Double) e.NewValue;
            spinEdit.incrementValue = newIncrement;
        }

        private static void incrementTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var spinEdit = (SpinEdit) d;
            var newType = (SpinIncrementType) e.NewValue;
            spinEdit.incrementType = newType;
        }

        private static void minValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var spinEdit = (SpinEdit) d;
            var newMinValue = (Double) e.NewValue;
            spinEdit.minValue = newMinValue;
            spinEdit.maxValue = Math.Max(newMinValue, spinEdit.maxValue);
            spinEdit.setDirectValue(spinEdit.innerValue, false);
        }
        
        private static void maxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var spinEdit = (SpinEdit) d;
            var newMaxValue = (Double) e.NewValue;
            spinEdit.maxValue = newMaxValue;
            spinEdit.minValue = Math.Min(newMaxValue, spinEdit.minValue);
            spinEdit.setDirectValue(spinEdit.innerValue, false);
        }

        private static void valueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SpinEdit) d).SetEditValue(e.NewValue);
        }

        private static void decimalDigitsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (SpinEdit) d;
            var newDigits = (Byte) e.NewValue;
            editor.decimalDigits = newDigits;
            editor.updateDisplayValue();
        }

        private static void textEditingEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = (SpinEdit) d;
            var newTextEditingEnabled = (Boolean) e.NewValue;
            editor.textEditingEnabled = newTextEditingEnabled;
            editor.updateEditorState();
        }

        private static bool isValidDoubleValue(Object value)
        {
            var d = (Double) value;
            return !Double.IsNaN(d) && !Double.IsInfinity(d);
        }

        private static bool isValidByteValue(Object value)
        {
            return Convert.ToByte(value).IsInRange(DecimalDigitsMin, DecimalDigitsMax);
        }

        private static Object constrainToRange(DependencyObject d, Object value)
        {
            var editor = (SpinEdit) d;
            var minimum = editor.MinValue;
            var num = (Double) value;
            if (num < minimum)
            {
                return minimum;
            }
            var maximum = editor.MaxValue;
            if (num > maximum)
            {
                return maximum;
            }
            return value;
        }

        private static Object coerceMaximum(DependencyObject d, Object value)
        {
            return Math.Max((Double) value, ((SpinEdit) d).minValue);
        }

        #endregion

        #endregion

        #region Overrides

        protected override void SetEditValue(Object value)
        {
            innerValue = Convert.ToDouble(value);
            updateDisplayValue();
        }

        protected override void SetReadOnly(Boolean readOnly)
        {
            innerReadOnly = readOnly;
            updateEditorState();
        }

        protected override Object CoerceEditValue(Object value)
        {
            var number = (Double) value;
            if (number < minValue)
            {
                return minValue;
            }
            if (number > maxValue)
            {
                return maxValue;
            }
            return value;
        }

        #endregion
    }
}
