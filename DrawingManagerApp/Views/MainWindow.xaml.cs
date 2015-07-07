using System.Windows;
using DrawingManagerApp.ViewModels;

namespace DrawingManagerApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DrawingManagerViewModel();
        }
    }
}
