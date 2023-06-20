using System;
using System.Collections.Generic;
using System.Threading;

namespace Inventor.Semantics.WPF
{
	public class ChangeController
	{
		#region Properties

		public bool HasChanges
		{ get { return _editHistory.Count > 0 && _currentEditPointer != _savedPointer; } }

		public bool CanUndo
		{ get { return _editHistory.Count > 0 && _currentEditPointer >= 0; } }

		public bool CanRedo
		{ get { return _editHistory.Count > 0 && _currentEditPointer < _editHistory.Count - 1; } }

		public event EventHandler Refreshed;

		private readonly List<IEditCommand> _editHistory = new List<IEditCommand>();
		private int _currentEditPointer = -1;
		private int _savedPointer = -1;

		#endregion

		private void raiseRefreshed()
		{
			var handler = Volatile.Read(ref Refreshed);
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		public void Perform(IEditCommand command)
		{
			command.Apply();

			_editHistory.RemoveRange(_currentEditPointer + 1, _editHistory.Count - _currentEditPointer - 1);

			_currentEditPointer = _editHistory.Count;
			_editHistory.Add(command);

			raiseRefreshed();
		}

		public void Undo()
		{
			_editHistory[_currentEditPointer].Rollback();

			_currentEditPointer--;

			raiseRefreshed();
		}

		public void Redo()
		{
			_currentEditPointer++;

			_editHistory[_currentEditPointer].Apply();

			raiseRefreshed();
		}

		public void ClearHistory()
		{
			_editHistory.Clear();
			_savedPointer = _currentEditPointer = -1;

			raiseRefreshed();
		}

		public void SaveHistory()
		{
			_savedPointer = _currentEditPointer;

			raiseRefreshed();
		}
	}
}
