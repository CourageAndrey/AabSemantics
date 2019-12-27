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

        public IEnumerable<ExtendedTreeNode> Children
        { get { return Сhildren; } }

        public Boolean IsSelected
        { get; set; }

        public Boolean IsExpanded
        { get; set; }

        protected readonly ObservableCollection<ExtendedTreeNode> Сhildren = new ObservableCollection<ExtendedTreeNode>();

        #endregion
    }
}
