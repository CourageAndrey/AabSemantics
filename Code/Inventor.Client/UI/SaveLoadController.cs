using System;
using System.Windows;
using System.Windows.Controls;

using Inventor.Core;

using Microsoft.Win32;

namespace Inventor.Client.UI
{
	public class SaveLoadController
	{
		public delegate void SaveHandler(IChangeable changeable, String fileName);
		public delegate IChangeable LoadHandler(String fileName);

		#region Properties

		public String FileName
		{ get { return _fileName; } }

		public Boolean IsChanged
		{ get { return _isChanged; } }

		private readonly Button _buttonNew, _buttonLoad, _buttonSave, _buttonSaveAs;
		private readonly Func<IChangeable> _createNew;
		private readonly LoadHandler _loadFromFile;
		private readonly SaveHandler _saveToFile;
		private readonly Func<OpenFileDialog> _createOpenDialog;
		private readonly Func<SaveFileDialog> _createSaveDialog;
		private readonly EventHandler _updateView;
		private IChangeable _entity;
		private Boolean _isChanged;
		private String _fileName;

		#endregion

		public SaveLoadController(Button buttonNew, Button buttonLoad, Button buttonSave, Button buttonSaveAs,
			Func<IChangeable> createNew, LoadHandler loadFromFile, SaveHandler saveToFile,
			Func<OpenFileDialog> createOpenDialog, Func<SaveFileDialog> createSaveDialog,
			EventHandler updateView,
			IChangeable entity = null)
		{
			if (buttonNew == null) throw new ArgumentNullException("buttonNew");
			if (buttonLoad == null) throw new ArgumentNullException("buttonLoad");
			if (buttonSave == null) throw new ArgumentNullException("buttonSave");
			if (buttonSaveAs == null) throw new ArgumentNullException("buttonSaveAs");
			if (createNew == null) throw new ArgumentNullException("createNew");
			if (loadFromFile == null) throw new ArgumentNullException("loadFromFile");
			if (saveToFile == null) throw new ArgumentNullException("saveToFile");
			if (createOpenDialog == null) throw new ArgumentNullException("createOpenDialog");
			if (createSaveDialog == null) throw new ArgumentNullException("createSaveDialog");
			if (updateView == null) throw new ArgumentNullException("updateView");

			_buttonNew = buttonNew;
			_buttonLoad = buttonLoad;
			_buttonSave = buttonSave;
			_buttonSaveAs = buttonSaveAs;
			_createNew = createNew;
			_loadFromFile = loadFromFile;
			_saveToFile = saveToFile;
			_createOpenDialog = createOpenDialog;
			_createSaveDialog = createSaveDialog;
			_updateView = updateView;

			_buttonNew.Click += newClick;
			_buttonLoad.Click += loadClick;
			_buttonSave.Click += saveClick;
			_buttonSaveAs.Click += saveAsClick;

			ChangeEntity(entity ?? createNew());
		}

		private void entityModified(Object sender, EventArgs eventArgs)
		{
			_isChanged = true;
			update();
		}

		private void update()
		{
			_buttonNew.IsEnabled = _buttonLoad.IsEnabled = _buttonSaveAs.IsEnabled = true;
			_buttonSave.IsEnabled = _isChanged;
			_updateView(this, EventArgs.Empty);
		}

		public void ChangeEntity(IChangeable newEntity, String newFileName = null)
		{
			if (newEntity == null) throw new ArgumentNullException("newEntity");

			if (_entity != null)
			{
				_entity.Changed -= entityModified;
			}
			_entity = newEntity;
			_entity.Changed += entityModified;

			_isChanged = false;
			_fileName = newFileName;
			update();
		}

		#region Save/Load buttons

		private void newClick(Object sender, RoutedEventArgs routedEventArgs)
		{
			ChangeEntity(_createNew());
		}

		private void loadClick(Object sender, RoutedEventArgs routedEventArgs)
		{
			var dialog = _createOpenDialog();
			dialog.FileName = _fileName;
			if (dialog.ShowDialog() == true)
			{
				ChangeEntity(_loadFromFile(dialog.FileName), dialog.FileName);
			}
		}

		private void saveClick(Object sender, RoutedEventArgs routedEventArgs)
		{
			if (String.IsNullOrEmpty(_fileName))
			{
				saveAsClick(sender, routedEventArgs);
			}
			else
			{
				_saveToFile(_entity, _fileName);
				_isChanged = false;
				update();
			}
		}

		private void saveAsClick(Object sender, RoutedEventArgs routedEventArgs)
		{
			var dialog = _createSaveDialog();
			dialog.FileName = _fileName;
			if (dialog.ShowDialog() == true)
			{
				_saveToFile(_entity, _fileName = dialog.FileName);
				_isChanged = false;
				update();
			}
		}

		#endregion
	}
}
