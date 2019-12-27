using System;
using System.Windows.Controls;

namespace Sef.Interfaces
{
    public interface IEditor
    {
        Object EditValue
        { get; set; }

        Boolean ReadOnly
        { get; set; }

        event EventHandler EditValueChanged;

        event EventHandler EditValueCommited;

        Control AsControl
        { get; }
    }

    public interface IEditor<T> : IEditor
    {
        T EditValueEx
        { get; set; }
    }
}
