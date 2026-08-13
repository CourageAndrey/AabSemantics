using System;
using System.Collections.Generic;
using System.Threading;

namespace AabSemantics.Extensions.WPF
{
	/// <summary>
	/// Undo/redo stack over <see cref="IEditCommand"/>. Performing a command clears the redo
	/// history, and <see cref="SaveHistory"/> marks the current position as saved.
	/// </summary>
	public class ChangeController
	{
		#region Properties

		/// <summary>Whether there are edits made since the last save.</summary>
		public bool HasChanges
		{ get { return _editHistory.Count > 0 && _currentEditPointer != _savedPointer; } }

		/// <summary>Whether an edit is available to undo.</summary>
		public bool CanUndo
		{ get { return _editHistory.Count > 0 && _currentEditPointer >= 0; } }

		/// <summary>Whether an undone edit is available to redo.</summary>
		public bool CanRedo
		{ get { return _editHistory.Count > 0 && _currentEditPointer < _editHistory.Count - 1; } }

		/// <summary>Raised after the history changes, so the UI can update its commands.</summary>
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

		/// <summary>Applies a command and pushes it onto the undo stack, discarding the redo history.</summary>
		/// <param name="command">Command to apply.</param>
		public void Perform(IEditCommand command)
		{
			command.Apply();

			_editHistory.RemoveRange(_currentEditPointer + 1, _editHistory.Count - _currentEditPointer - 1);

			_currentEditPointer = _editHistory.Count;
			_editHistory.Add(command);

			raiseRefreshed();
		}

		/// <summary>Reverses the most recent edit and moves it onto the redo stack.</summary>
		public void Undo()
		{
			_editHistory[_currentEditPointer].Rollback();

			_currentEditPointer--;

			raiseRefreshed();
		}

		/// <summary>Re-applies the most recently undone edit.</summary>
		public void Redo()
		{
			_currentEditPointer++;

			_editHistory[_currentEditPointer].Apply();

			raiseRefreshed();
		}

		/// <summary>Discards both stacks, e.g. after loading another knowledge base.</summary>
		public void ClearHistory()
		{
			_editHistory.Clear();
			_savedPointer = _currentEditPointer = -1;

			raiseRefreshed();
		}

		/// <summary>Marks the current position as saved, so <see cref="HasChanges"/> becomes <c>false</c>.</summary>
		public void SaveHistory()
		{
			_savedPointer = _currentEditPointer;

			raiseRefreshed();
		}
	}
}
