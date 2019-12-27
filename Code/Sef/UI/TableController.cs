using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Sef.Interfaces;
using Sef.Localization;

namespace Sef.UI
{
    public class TableController<ItemT>
        where ItemT : IEditable<ItemT>, new()
    {
        private readonly DataGrid table;
        private Func<IEnumerable<ItemT>> sourceGetter;
        private readonly ObservableCollection<ItemT> innerSource;
        private readonly ObservableCollection<ICommand> additionalMenuCommands;
        private readonly CommandBindingCollection parentCommandsList;
        private CommandBinding commandBindingAdd;
        private CommandBinding commandBindingEdit;
        private CommandBinding commandBindingView;
        private CommandBinding commandBindingDelete;
        private CommandBinding commandBindingClear;
        private ICommand commandAdd;
        private ICommand commandEdit;
        private ICommand commandView;
        private ICommand commandRemove;
        private ICommand commandClear;
        private readonly MenuItem menuItemAdd;
        private readonly MenuItem menuItemEdit;
        private readonly MenuItem menuItemView;
        private readonly MenuItem menuItemDelete;
        private readonly MenuItem menuItemClear;
        private Func<ItemT> methodCreateNew;
        private Action<ItemT> methodAddToList;
        private Action<ItemT> methodRemoveFromList;
        private readonly ContextMenu contextMenu;
        private readonly List<ItemT> selectedItems;
        private IEditor<ItemT> editor;

        public DataGrid Grid
        { get { return table; } }

        public Func<IEnumerable<ItemT>> SourceGetter
        {
            get { return sourceGetter; }
            set
            {
                sourceGetter = value;
                Reload();
            }
        }

        public IEditor<ItemT> EditorControl
        {
            get { return editor; }
            set { editor = value; }
        }

        public ImageSource EditorDialogIcon
        { get; set; }

        public Func<ItemT> MethodCreateNew
        {
            get { return methodCreateNew; }
            set
            {
                methodCreateNew = value;
                updateCommands();
            }
        }

        public Action<ItemT> MethodAddToList
        {
            get { return methodAddToList; }
            set
            {
                methodAddToList = value;
                updateCommands();
            }
        }

        public Action<ItemT> MethodRemoveFromList
        {
            get { return methodRemoveFromList; }
            set
            {
                methodRemoveFromList = value;
                updateCommands();
            }
        }

        public ICommand CommandAdd
        {
            get { return commandAdd; }
            set
            {
                removeOldCommand(commandBindingAdd, menuItemAdd);
                commandAdd = value;
                commandBindingAdd = addNewCommand(commandAdd, menuItemAdd, commandAdd_Executed, commandAdd_CanExecute);
                reorderContextMenu();
            }
        }

        public ICommand CommandEdit
        {
            get { return commandEdit; }
            set
            {
                removeOldCommand(commandBindingEdit, menuItemEdit);
                commandEdit = value;
                commandBindingEdit = addNewCommand(commandEdit, menuItemEdit, commandEdit_Executed, commandEdit_CanExecute);
                reorderContextMenu();
            }
        }

        public ICommand CommandView
        {
            get { return commandView; }
            set
            {
                removeOldCommand(commandBindingView, menuItemView);
                commandView = value;
                commandBindingView = addNewCommand(commandView, menuItemView, commandView_Executed, commandView_CanExecute);
                reorderContextMenu();
            }
        }

        public ICommand CommandRemove
        {
            get { return commandRemove; }
            set
            {
                removeOldCommand(commandBindingDelete, menuItemDelete);
                commandRemove = value;
                commandBindingDelete = addNewCommand(commandRemove, menuItemDelete, commandDelete_Executed, commandDelete_CanExecute);
                reorderContextMenu();
            }
        }

        public ICommand CommandClear
        {
            get { return commandClear; }
            set
            {
                removeOldCommand(commandBindingClear, menuItemClear);
                commandClear = value;
                commandBindingClear = addNewCommand(commandClear, menuItemClear, commandClear_Executed, commandClear_CanExecute);
                reorderContextMenu();
            }
        }

        public IList<ICommand> AdditionalMenuCommands
        { get { return additionalMenuCommands; } }

        public TableController(DataGrid dataGrid, CommandBindingCollection commands)
        {
            innerSource = new ObservableCollection<ItemT>();
            contextMenu = new ContextMenu();
            menuItemAdd = new MenuItem();
            menuItemEdit = new MenuItem();
            menuItemView = new MenuItem();
            menuItemDelete = new MenuItem();
            menuItemClear = new MenuItem();
            selectedItems = new List<ItemT>();
            additionalMenuCommands = new ObservableCollection<ICommand>();

            contextMenu.Opened += contextMenu_Opened;
            additionalMenuCommands.CollectionChanged += additionalMenuCommands_CollectionChanged;

            table = dataGrid;
            table.AutoGenerateColumns = false;
            table.ContextMenu = contextMenu;
            table.ItemsSource = innerSource;
            table.SelectionChanged += table_SelectionChanged;
            table.MouseDoubleClick += table_MouseDoubleClick;

            parentCommandsList = commands;
        }

        public void BeginInitialize()
        {
            initializing = true;
        }

        public void EndInitialize()
        {
            initializing = false;
            reorderContextMenu();
        }

        private void updateCommands()
        {
            if (!initializing)
            {
                UiHelper.UpdateCommandsEnabled();
            }
        }

        private bool initializing;

        #region Selection

        public event EventHandler SelectionChanged;

        private void table_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            selectedItems.Clear();
            selectedItems.AddRange(table.SelectedItems.OfType<ItemT>());
            //updateCommands();
            var handler = Volatile.Read(ref SelectionChanged);
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public IEnumerable<ItemT> SelectedItems
        { get { return selectedItems; } }

        #endregion

        #region Commands

        #region Execute handlers

        private void commandAdd_Executed(Object sender, ExecutedRoutedEventArgs e)
        {
            var newItem = methodCreateNew();
            Boolean add;
            if (editor != null)
            {
                editor.EditValueEx = newItem;
                add = CommonDialog.ShowControlDialog(editor.AsControl, Language.Current.Editor.ModeCreate, EditorDialogIcon) == true;
            }
            else
            {
                add = true;
            }
            if (add)
            {
                methodAddToList(newItem);
                Reload();
            }
        }

        private void commandEdit_Executed(Object sender, ExecutedRoutedEventArgs e)
        {
            var currentItem = selectedItems[0];
            editor.EditValueEx = currentItem.Clone();
            editor.ReadOnly = false;
            if (CommonDialog.ShowControlDialog(editor.AsControl, Language.Current.Editor.ModeEdit, EditorDialogIcon) == true)
            {
                currentItem.UpdateFrom(editor.EditValueEx);
                Reload();
            }
        }

        private void commandView_Executed(Object sender, ExecutedRoutedEventArgs e)
        {
            var currentItem = selectedItems[0];
            editor.EditValueEx = currentItem;
            editor.ReadOnly = true;
            CommonDialog.ShowControlDialog(editor.AsControl, Language.Current.Editor.ModeView, EditorDialogIcon);
        }

        private void commandDelete_Executed(Object sender, ExecutedRoutedEventArgs e)
        {
            if (suppressDeleteQuestion || MessageBox.Show(
                Language.Current.Editor.DeletePromt,
                Language.Current.Common.Question,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                foreach (var item in selectedItems)
                {
                    methodRemoveFromList(item);
                }
                Reload();
            }
        }

        private bool suppressDeleteQuestion;

        private void commandClear_Executed(Object sender, ExecutedRoutedEventArgs e)
        {
            if (MessageBox.Show(
                Language.Current.Editor.DeletePromt,
                Language.Current.Common.Question,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                suppressDeleteQuestion = true;
                selectedItems.Clear();
                selectedItems.AddRange(innerSource);
                commandBindingDelete.Command.Execute(null);
                suppressDeleteQuestion = false;
            }
            Reload();
        }

        #endregion

        #region CanExecute checkers

        private void commandAdd_CanExecute(Object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (methodAddToList != null);
        }

        private void commandEdit_CanExecute(Object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (editor != null) && selectedItems.Any();
        }

        private void commandView_CanExecute(Object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (editor != null) && selectedItems.Any();
        }

        private void commandDelete_CanExecute(Object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (methodRemoveFromList != null) && selectedItems.Any();
        }

        private void commandClear_CanExecute(Object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (methodRemoveFromList != null) && innerSource.Any();
        }

        #endregion

        private void table_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (commandBindingView != null && commandBindingView.Command.CanExecute(null))
            {
                commandBindingView.Command.Execute(null);
            }
        }

        #endregion

        public void Reload()
        {
            innerSource.Clear();
            if (sourceGetter != null)
            {
                foreach (var item in sourceGetter())
                {
                    innerSource.Add(item);
                }
            }
        }

        private void removeOldCommand(CommandBinding commandBinding, MenuItem menuItem)
        {
            if (commandBinding != null && parentCommandsList.Contains(commandBinding))
            {
                parentCommandsList.Remove(commandBinding);
            }
            if (menuItem != null)
            {
                contextMenu.Items.Remove(menuItem);
            }
        }

        private CommandBinding addNewCommand(ICommand command, MenuItem menuItem,
            ExecutedRoutedEventHandler executed,
            CanExecuteRoutedEventHandler canExecuted)
        {
            CommandBinding commandBinding = null;
            if (command is ILocalizable)
            {
                (command as ILocalizable).Localize();
            }
            menuItem.Icon = (command is IHasImage) ? (command as IHasImage).Image.ToSource() : null;
            menuItem.Command = command;
            if (command != null)
            {
                commandBinding = new CommandBinding(command);
                commandBinding.Executed += executed;
                commandBinding.CanExecute += canExecuted;
                parentCommandsList.Add(commandBinding);
                contextMenu.Items.Add(menuItem);
            }
            return commandBinding;
        }

        private void reorderContextMenu()
        {
            if (!initializing)
            {
                contextMenu.Items.Clear();
                updateMenuItem(commandAdd, commandBindingAdd, menuItemAdd);
                updateMenuItem(commandEdit, commandBindingEdit, menuItemEdit);
                updateMenuItem(commandView, commandBindingView, menuItemView);
                updateMenuItem(commandRemove, commandBindingDelete, menuItemDelete);
                updateMenuItem(commandClear, commandBindingClear, menuItemClear);
                if (contextMenu.Items.Count > 0 && additionalMenuCommands.Count > 0)
                {
                    contextMenu.Items.Add(new Separator());
                }
                foreach (var command in additionalMenuCommands)
                {
                    contextMenu.Items.Add(command.ConvertToMenuItem());
                }
            }
            updateCommands();
        }

        private void updateMenuItem(ICommand command, CommandBinding commandBinding, MenuItem menuItem)
        {
            if (commandBinding != null)
            {
                menuItem.UpdateFromCommand(command);
                contextMenu.Items.Add(menuItem);
            }
        }

        private void additionalMenuCommands_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            reorderContextMenu();
        }

        private void contextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (additionalMenuCommands.Count == 0 &&
                commandBindingAdd == null &&
                commandBindingEdit == null &&
                commandBindingView == null &&
                commandBindingDelete == null &&
                commandBindingClear == null)
            {
                contextMenu.IsOpen = false;
            }
        }
    }
}
