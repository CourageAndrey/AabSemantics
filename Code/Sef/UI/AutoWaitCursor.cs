using System.Windows.Input;

using Sef.Common;

namespace Sef.UI
{
    public class AutoWaitCursor : DisposableBase
    {
        private readonly bool skipped;
        private readonly Cursor oldCursor;

        public AutoWaitCursor()
        {
            oldCursor = Mouse.OverrideCursor;
            skipped = (oldCursor != null) && oldCursor.Equals(Cursors.Wait);
            if (!skipped)
            {
                Mouse.OverrideCursor = Cursors.Wait;
            }
        }

        protected override void Free()
        {
            if (!skipped)
            {
                Mouse.OverrideCursor = oldCursor;
            }
        }
    }
}
