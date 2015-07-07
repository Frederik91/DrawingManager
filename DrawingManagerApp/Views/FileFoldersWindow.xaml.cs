using System.Windows;
using DrawingManagerApp.ViewModels;

namespace DrawingManagerApp.Views
{
    /// <summary>
    /// Interaction logic for FileFoldersWindow.xaml
    /// </summary>
    public partial class FileFoldersWindow : Window
    {
        public FileFoldersWindow()
        {

            InitializeComponent();
            DataContext = new FileFoldersViewModel();
        }
    }
}
