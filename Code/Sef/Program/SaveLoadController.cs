using System;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Win32;

namespace Sef.Program
{
    public class SaveLoadController
    {
        public delegate void SaveHandler(IChangeable changeable, String fileName);
        public delegate IChangeable LoadHandler(String fileName);

        #region Properties

        public String FileName
        { get { return fileName; } }

        public Boolean IsChanged
        { get { return isChanged; } }

        private readonly Button buttonNew, buttonLoad, buttonSave, buttonSaveAs;
        private readonly Func<IChangeable> createNew;
        private readonly LoadHandler loadFromFile;
        private readonly SaveHandler saveToFile;
        private readonly Func<OpenFileDialog> createOpenDialog;
        private readonly Func<SaveFileDialog> createSaveDialog;
        private readonly EventHandler updateView;
        private IChangeable entity;
        private Boolean isChanged;
        private String fileName;

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

            this.buttonNew = buttonNew;
            this.buttonLoad = buttonLoad;
            this.buttonSave = buttonSave;
            this.buttonSaveAs = buttonSaveAs;
            this.createNew = createNew;
            this.loadFromFile = loadFromFile;
            this.saveToFile = saveToFile;
            this.createOpenDialog = createOpenDialog;
            this.createSaveDialog = createSaveDialog;
            this.updateView = updateView;

            this.buttonNew.Click += newClick;
            this.buttonLoad.Click += loadClick;
            this.buttonSave.Click += saveClick;
            this.buttonSaveAs.Click += saveAsClick;

            ChangeEntity(entity ?? createNew());
        }

        private void entityModified(Object sender, EventArgs eventArgs)
        {
            isChanged = true;
            update();
        }

        private void update()
        {
            buttonNew.IsEnabled = buttonLoad.IsEnabled = buttonSaveAs.IsEnabled = true;
            buttonSave.IsEnabled = isChanged;
            updateView(this, EventArgs.Empty);
        }

        public void ChangeEntity(IChangeable newEntity, String newFileName = null)
        {
            if (newEntity == null) throw new ArgumentNullException("newEntity");

            if (entity != null)
            {
                entity.Changed -= entityModified;
            }
            entity = newEntity;
            entity.Changed += entityModified;

            isChanged = false;
            fileName = newFileName;
            update();
        }

        #region Save/Load buttons

        private void newClick(Object sender, RoutedEventArgs routedEventArgs)
        {
            ChangeEntity(createNew());
        }

        private void loadClick(Object sender, RoutedEventArgs routedEventArgs)
        {
            var dialog = createOpenDialog();
            dialog.FileName = fileName;
            if (dialog.ShowDialog() == true)
            {
                ChangeEntity(loadFromFile(dialog.FileName), dialog.FileName);
            }
        }

        private void saveClick(Object sender, RoutedEventArgs routedEventArgs)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                saveAsClick(sender, routedEventArgs);
            }
            else
            {
                saveToFile(entity, fileName);
                isChanged = false;
                update();
            }
        }

        private void saveAsClick(Object sender, RoutedEventArgs routedEventArgs)
        {
            var dialog = createSaveDialog();
            dialog.FileName = fileName;
            if (dialog.ShowDialog() == true)
            {
                saveToFile(entity, fileName = dialog.FileName);
                isChanged = false;
                update();
            }
        }

        #endregion
    }
}
