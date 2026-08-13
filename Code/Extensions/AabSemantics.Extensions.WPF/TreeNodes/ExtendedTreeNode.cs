using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows.Media;

namespace AabSemantics.Extensions.WPF.TreeNodes
{
	/// <summary>Base node of the knowledge tree, notifying the view when its state changes.</summary>
	public abstract class ExtendedTreeNode : INotifyPropertyChanged
	{
		#region Properties

		/// <summary>Caption shown in the tree.</summary>
		public abstract String Text
		{ get; }

		/// <summary>Tooltip shown for the node.</summary>
		public abstract String Tooltip
		{ get; }

		/// <summary>Icon shown next to the caption.</summary>
		public abstract ImageSource Icon
		{ get; }

		/// <summary>Child nodes.</summary>
		public ObservableCollection<ExtendedTreeNode> Children
		{ get; } = new ObservableCollection<ExtendedTreeNode>();

		/// <summary>Whether the node is selected; bound two-way to the tree.</summary>
		public System.Boolean IsSelected
		{ get; set; }

		/// <summary>Whether the node is expanded; bound two-way to the tree.</summary>
		public System.Boolean IsExpanded
		{ get; set; }

		#endregion

		#region Implementation of INotifyPropertyChanged

		/// <summary>Raised when a bound property changes.</summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>Tells the view to re-read every bound property of this node.</summary>
		public void RefreshView()
		{
			var handler = Volatile.Read(ref PropertyChanged);
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(null));
			}
		}

		#endregion
	}
}
