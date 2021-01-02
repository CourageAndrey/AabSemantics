using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Sef.UI
{
    public abstract class ExtendedTreeNode
    {
        #region Properties

        public abstract String Text
        { get; }

        public abstract String Tooltip
        { get; }

        public abstract ImageSource Icon
        { get; }

        public ICollection<ExtendedTreeNode> Children
        { get; } = new ObservableCollection<ExtendedTreeNode>();

        public Boolean IsSelected
        { get; set; }

        public Boolean IsExpanded
        { get; set; }

        #endregion
    }
}
