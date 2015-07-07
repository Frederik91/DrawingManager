using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DrawingManagerApp.ViewModels;
using DrawingManagerApp.Views;

namespace DrawingManagerApp.Commands
{
    class FileFoldersCommand : ICommand
    {
        private readonly MainViewModel _mainViewModel;


        public FileFoldersCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        #region ICommand members



        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            FileFoldersWindow window = new FileFoldersWindow();
            _mainViewModel.OpenFileFolderWindow(window);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }
        #endregion
    }
}
