using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace GameOfLife.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DispatcherHelper.Initialize();
        }
    }
}
