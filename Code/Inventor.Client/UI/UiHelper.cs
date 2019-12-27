using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Inventor.Client.UI
{
    public static class UiHelper
    {
        public static void ExecuteWithItem(this ItemsControl parentContainer, List<object> path, Action<TreeViewItem> code)
        {
            if (path.Count > 0)
            {
                var head = path.First();
                var tail = path.GetRange(1, path.Count - 1);
                var item = parentContainer.ItemContainerGenerator.ContainerFromItem(head) as TreeViewItem;
                if (item != null)
                {
                    item.IsExpanded = true;
                    if (item.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                    {
                        item.ItemContainerGenerator.StatusChanged += delegate
                        {
                            ExecuteWithItem(item, tail, code);
                        };
                    }
                    else
                    {
                        ExecuteWithItem(item, tail, code);
                    }
                }
            }
            else
            {
                code(parentContainer as TreeViewItem);
            }
        }
    }
}
