using Inventor.Core;

using Sef.Interfaces;

namespace Inventor.Client.UI
{
    public partial class ConfigurationControl : IEditor<InventorConfiguration>
    {
        public ConfigurationControl()
        {
            InitializeComponent();
        }

        #region Overrides

        protected override void SetEditValue(object value)
        {
            EditValueEx = (InventorConfiguration) value;
        }

        protected override void SetReadOnly(bool readOnly)
        {
            contextControl.IsEnabled = !readOnly;
        }

        #endregion

        public InventorConfiguration EditValueEx
        {
            get { return contextControl.DataContext as InventorConfiguration; }
            set { contextControl.DataContext = value; }
        }
    }
}
