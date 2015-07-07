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
        private readonly DrawingManagerViewModel _mainViewModel;


        public FileFoldersCommand(DrawingManagerViewModel mainViewModel)
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
            _mainViewModel.OpenFileFolderWindow();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }
        #endregion
    }
}
