using System.Windows;
using GalaSoft.MvvmLight.Threading;
using MahApps.Metro.Controls;

namespace GameOfLife.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DispatcherHelper.Initialize();
        }
    }
}
